import datetime
import json
import os
import socket
from _thread import start_new_thread

from apscheduler.schedulers.background import BackgroundScheduler

import Actor
import Constants

# Log-Settings
log_data_requests = False

# Get current directory
c_dir = os.path.realpath('.')

# Get actors from json
actors_file_name = c_dir + '\\data\\actors.json'
actors_file = open(actors_file_name, 'r')
actors_list = json.load(actors_file)
actors_file.close()

actors = {}
for i in actors_list:
    actors[i] = None

max_clients = 10
port = 2222


def connect_missing():
    for missing_actor in actors:
        if actors[missing_actor] is not None:
            b = actors[missing_actor].connected
            try:
                actors[missing_actor].connect()
                if not b:
                    now = datetime.datetime.now()
                    c_t1 = "[" + str(now.day) + "." + str(now.month) + "." + str(now.year) + ", " \
                           + str(now.hour) + ":" + str(now.minute) + ":" + str(now.second) + "]"
                    connect_missing_pre = "[SERVER]                 "
                    print(c_t1, connect_missing_pre, missing_actor, "connected")
            except Exception:
                pass


def update_connection_states():
    for updating_actor in actors:
        b = actors[updating_actor].connected
        b2 = actors[updating_actor].check_connection()
        if not b2 and b:
            now = datetime.datetime.now()
            c_t2 = "[" + str(now.day) + "." + str(now.month) + "." + str(now.year) + ", " \
                   + str(now.hour) + ":" + str(now.minute) + ":" + str(now.second) + "]"
            update_pre = "[SERVER]                 "
            print(c_t2, update_pre, updating_actor, "disconnected")


def get_info():
    info = ""
    for a in actors:
        a_info = a + "_" + \
                 actors[a].ip + "_" + \
                 str(actors[a].port) + "_" + \
                 str(actors[a].state) + "_" + \
                 str(actors[a].state_count) + "_" + \
                 str(actors[a].state_names) + "_" + \
                 str(actors[a].connected)
        info += a_info
        info += "+"
    info = info[0:len(info) - 1]
    return info


def client_thread(t_conn):
    pre_client = "[" + str(t_conn.getpeername()[0]) + ":" + str(t_conn.getpeername()[1]) + "]  "
    while True:
        # noinspection PyBroadException
        try:
            data = t_conn.recv(Constants.REQUEST_LENGTH)
            if not data:
                break
            now = datetime.datetime.now()
            c_msg = str(data, "utf8")
            c_t3 = "[" + str(now.day) + "." + str(now.month) + "." + str(now.year) + ", " \
                   + str(now.hour) + ":" + str(now.minute) + ":" + str(now.second) + "] "
            pre_c = c_t3 + pre_client
            while(len(pre_c)) < 47:
                pre_c += " "

            # UI_CLIENT_DATA_REQUEST
            if c_msg == Constants.UI_CLIENT_DATA_REQUEST:
                if log_data_requests:
                    print(pre_c, "Received UI_CLIENT_DATA_REQUEST")
                msg = get_info()
                while len(bytes(msg, "utf8")) < Constants.SERVER_ANSWER_LENGTH:
                    msg += "#"
                t_conn.sendall(bytes(msg, "utf8"))

            # UI_CLIENT_SENSOR_DATA_REQUEST
            if c_msg == Constants.UI_CLIENT_SENSOR_DATA_REQUEST:
                # TODO Send Sensor-Data requested by external rpi-Server
                pass

            # UI_CLIENT_COMMAND_IDENTIFIER
            if Constants.UI_CLIENT_COMMAND_IDENTIFIER in c_msg:
                c_msg = c_msg.replace("#", "")
                print(pre_c, "Received UI_CLIENT_COMMAND:", c_msg)
                c_parts = c_msg.split("_")
                c_cmd_name = c_parts[1]
                c_cmd_n_state = c_parts[2]
                actors[c_cmd_name].switch_to(int(c_cmd_n_state))
                msg = get_info()
                while len(bytes(msg, "utf8")) < Constants.SERVER_ANSWER_LENGTH:
                    msg += "#"
                t_conn.sendall(bytes(msg, "utf8"))

            # UI_CLIENT_ADD_DEVICE_IDENTIFIER
            if Constants.UI_CLIENT_ADD_DEVICE_IDENTIFIER in c_msg:
                c_msg = c_msg.replace("#", "")
                print(pre_c, "Received UI_CLIENT_ADD_DEVICE:", c_msg)
                c_parts = c_msg.split("_")
                n_name = c_parts[1]
                n_ip = c_parts[2]
                n_port = int(c_parts[3])
                n_states = int(c_parts[4])
                if n_name not in actors:
                    if len(c_parts) == 5 + n_states:
                        n_state_names = c_parts[5:len(c_parts)]
                        actors[n_name] = Actor.Actor(n_ip,
                                                     n_port,
                                                     n_states,
                                                     n_name,
                                                     n_state_names)
                    else:
                        n_state_names = None
                        actors[n_name] = Actor.Actor(n_ip,
                                                     n_port,
                                                     n_states,
                                                     n_name)
                    try:
                        actors[n_name].connect()
                        file = open(actors_file_name, 'r')
                        all_text = ""
                        line = file.readline()
                        while line:
                            all_text += line
                            line = file.readline()
                        file.close()
                        all_text = all_text[0:len(all_text) - 2]
                        all_text += ",\n"
                        all_text += "  \"" + n_name + "\": {\n"
                        all_text += "    \"ip\": \"" + n_ip + "\",\n"
                        all_text += "    \"port\": " + str(n_port) + ",\n"
                        all_text += "    \"state_count\": " + str(n_states) + ",\n"
                        if n_state_names is not None:
                            all_text += "    \"state_names\": ["
                            for name in n_state_names:
                                all_text += "\"" + name + "\", "
                        all_text = all_text[0:len(all_text) - 2]
                        if n_state_names:
                            all_text += "]\n"
                        all_text += "  }\n"
                        all_text += "}"
                        file = open(actors_file_name, "w")
                        file.write(all_text)
                        file.close()
                    except:
                        print("Actor not found.")
                        del actors[n_name]
                msg = get_info()
                while len(bytes(msg, "utf8")) < Constants.SERVER_ANSWER_LENGTH:
                    msg += "#"
                t_conn.sendall(bytes(msg, "utf8"))

        except Exception as e:
            print("[ERROR] " + str(e))

    t_conn.close()


for actor_name in actors:
    actor_info = actors_list[actor_name]
    actor_ip = actor_info["ip"]
    actor_port = actor_info["port"]
    if "state_names" in actor_info:
        actors[actor_name] = Actor.Actor(actor_ip,
                                         actor_port,
                                         actor_info["state_count"],
                                         actor_name,
                                         actor_info["state_names"])
    else:
        actors[actor_name] = Actor.Actor(actor_ip,
                                         actor_port,
                                         actor_name,
                                         actor_info["state_count"])

# CONNECT TO ACTORS
pre = "[SERVER]                 "
for actor in actors:
    now = datetime.datetime.now()
    try:
        actors[actor].connect()
        c_t = "[" + str(now.day) + "." + str(now.month) + "." + str(now.year) + ", " \
              + str(now.hour) + ":" + str(now.minute) + ":" + str(now.second) + "]"
        print(c_t, pre, actor, "connected")
    except Exception:
        c_t = "[" + str(now.day) + "." + str(now.month) + "." + str(now.year) + ", " \
              + str(now.hour) + ":" + str(now.minute) + ":" + str(now.second) + "]"
        print(c_t, pre, actor, "offline")

# START UPDATE-TIMERS
t_delay = 6
scheduler = BackgroundScheduler()
scheduler.add_job(update_connection_states, 'interval', seconds=t_delay)
scheduler.add_job(connect_missing, 'interval', seconds=t_delay)
scheduler.start()


# START UI-SERVER
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(('', port))
server_socket.listen(max_clients)
now = datetime.datetime.now()
c_t = "[" + str(now.day) + "." + str(now.month) + "." + str(now.year) + ", " \
      + str(now.hour) + ":" + str(now.minute) + ":" + str(now.second) + "]"
print(c_t, pre, "Server started")

while True:
    conn, addr = server_socket.accept()
    start_new_thread(client_thread, (conn,))
