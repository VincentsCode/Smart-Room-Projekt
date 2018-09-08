import random
import socket

import Constants

from flask import *
from app import app

s_ip = "localhost"
s_port = 2222
socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
socket.connect((s_ip, s_port))


card = '<div class="grid-item"><div style="width: 100%; height: 100%;"><h2>{} ({})</h2><hr><form action="" style="text-align: left">{}</form></div></div>'

class Actor:
    def __init__(self, name, ip, port, state, state_count, state_names, connected):
        self.name = str(name)
        self.ip = str(ip)
        self.port = int(port)
        self.state_count = int(state_count)
        self.state_names = state_names.replace("[", "").replace("]", "").replace("'", "").split(", ")
        self.state = int(state)
        self.connected = bool(connected)

    def __repr__(self):
        return "<Actor.AdvancedSwitch object: ip={}, port={}, connected={}, state={}, state_count={}, state_names={}>"\
            .format(self.ip, self.port, self.connected, self.state, self.state_count, str(self.state_names))


actors = []


@app.route('/')
@app.route('/index')
def index():
    return render_template('index.html')


@app.route('/current')
def current():
    def get_data():
        global actors
        msg = Constants.UI_CLIENT_DATA_REQUEST
        while len(bytes(msg, "utf8")) < Constants.REQUEST_LENGTH:
            msg += "#"
        socket.sendall(bytes(msg, "utf8"))
        answer = socket.recv(Constants.SERVER_ANSWER_LENGTH)
        s_a = str(answer, "utf8")
        s_a = s_a.replace('#', '')

        s_split = s_a.split("+")

        actors = []
        for i in range(len(s_split)):
            x = s_split[i].split("_")
            actors.append(Actor(x[0], x[1], x[2], x[3], x[4], x[5], x[6]))

        print(actors)

        ret = ""
        for a in actors:
            ret += card

        return "data:" + ret + "\n\n"

    return Response(get_data(), mimetype='text/event-stream')
