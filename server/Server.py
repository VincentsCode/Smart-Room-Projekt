import json
import os
import socket
import traceback
from _thread import start_new_thread

import Actor
import Constants

c_dir = os.path.realpath('.')
actors_file_name = c_dir + '\\data\\actors.json'

max_clients = 10
port = 2222


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

    return info


def client_thread(t_conn):
    while True:
        # noinspection PyBroadException
        try:
            data = t_conn.recv(32)
            if not data:
                break
            c_msg = str(data, "utf8")

            # REQUESTS
            # UI_CLIENT_DATA_REQUEST
            if c_msg == Constants.UI_CLIENT_DATA_REQUEST:
                msg = get_info()
                while len(bytes(msg, "utf8")) < 2048:
                    msg += "#"
                t_conn.sendall(bytes(msg, "utf8"))

            # UI_CLIENT_DEVICES_LOG_REQUEST
            if c_msg == Constants.UI_CLIENT_DEVICES_LOG_REQUEST:
                # TODO
                # Send Log from all Devices
                pass

            # UI_CLIENT_MOVEMENT_LOG_REQUEST
            if c_msg == Constants.UI_CLIENT_MOVEMENT_LOG_REQUEST:
                # TODO
                # Send Log from Movement-Recognition
                pass

            # UI_CLIENT_SENSOR_DATA_REQUEST
            if c_msg == Constants.UI_CLIENT_SENSOR_DATA_REQUEST:
                # TODO
                # Send Sensor-Data
                pass

            # UI_CLIENT_RNN_HABITS_REQUEST
            if c_msg == Constants.UI_CLIENT_RNN_HABITS_REQUEST:
                # TODO
                # Send RNN-Results (Habits)
                pass

            # COMMANDS
            # UI_CLIENT_COMMAND_IDENTIFIER
            if Constants.UI_CLIENT_COMMAND_IDENTIFIER in c_msg:
                c_msg = c_msg.replace("#", "")
                print(c_msg)
                c_parts = c_msg.split("_")
                print(c_parts)
                c_cmd_name = c_parts[1]
                c_cmd_n_state = c_parts[2]
                print("State 0", actors[c_cmd_name])
                actors[c_cmd_name].switch_to(int(c_cmd_n_state))
                print("State 1", actors[c_cmd_name])
                msg = get_info()
                while len(bytes(msg, "utf8")) < 2048:
                    msg += "#"
                t_conn.sendall(bytes(msg, "utf8"))

            # UI_CLIENT_SYSTEM_COMMAND_IDENTIFIER
            if Constants.UI_CLIENT_SYSTEM_COMMAND_IDENTIFIER in c_msg:
                # TODO
                # Enable or disable system
                # change vars
                # add to info
                pass

            # UI_CLIENT_ADD_DEVICE_IDENTIFIER
            if Constants.UI_CLIENT_ADD_DEVICE_IDENTIFIER in c_msg:
                # TODO
                # Add Object to actors.json
                # Reload Objects
                # return new list
                pass

        except Exception:
            traceback.print_exc()
            print("Client disconnected")

    t_conn.close()


# GET ACTORS FROM JSON
actors_file = open(actors_file_name, 'r')
actors_list = json.load(actors_file)
actors_file.close()

actors = {}
for i in actors_list:
    actors[i] = None

for actor_name in actors:
    actor_info = actors_list[actor_name]
    actor_ip = actor_info["ip"]
    actor_port = actor_info["port"]
    if "state_names" in actor_info:
        actors[actor_name] = Actor.Actor(actor_ip,
                                         actor_port,
                                         actor_info["state_count"],
                                         actor_info["state_names"])
    else:
        actors[actor_name] = Actor.Actor(actor_ip,
                                         actor_port,
                                         actor_info["state_count"])

# CONNECT TO ACTORS
for actor in actors:
    actors[actor].connect()

# START UI-SERVER
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(('', port))
server_socket.listen(max_clients)

while True:
    conn, addr = server_socket.accept()
    print(addr[0] + ":" + str(addr[1]), "connected")
    start_new_thread(client_thread, (conn,))
