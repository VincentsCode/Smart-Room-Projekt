import socket

import Constants
import time
import os
import datetime


# Actor WITH N STATES
class Actor:
    """
    m_IP (string): IP of the Actor
    m_port (int): Port of the Actor
    state_count (int): Number of possible states
    state_names (string-array): Names of the States e.g. ["An", "StandBy", "AUS"]
    state (int): current state
    connected (boolean): is the device connected

    connect(): connects to the actor
    switch(): circles through states
    switch_to(int): switches to a certain state
    get_state(boolean): returns the current state as text or int or -1 if disconnected
    """
    def __init__(self, m_ip, m_port, state_count, name, state_names=None):
        if state_names is None:
            state_names = []
            for i in range(state_count):
                state_names[i] = str(i)
        self.ip = m_ip
        self.port = m_port
        self.state_count = state_count
        self.state_names = state_names
        self.state = 0
        self.socket = None
        self.connected = False
        self.name = name

        # Create Log-File
        now = datetime.datetime.now()
        folder_name = str(now.day) + "_" + str(now.month) + "_" + str(now.year)
        d = os.path.realpath('.')
        file = open(d + "\\log\\" + folder_name + "\\" + self.name + ".log", "a")
        file.close()

    def __repr__(self):
        return "<Actor.AdvancedSwitch object: ip={}, port={}, connected={}, state={}, state_count={}, state_names={}>"\
            .format(self.ip, self.port, self.connected, self.state, self.state_count, self.state_names)

    def connect(self):
        if not self.connected:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.connect((self.ip, self.port))
            self.connected = True
            self.switch_to(0)

    def switch(self):
        if self.connected:
            if (self.state + 1) <= self.state_count:
                self.switch_to(self.state + 1)
                self.state = self.state + 1
            else:
                self.switch_to(0)
                self.state = 0
        else:
            return False

    def switch_to(self, n_state):
        try:
            if self.connected and n_state <= self.state_count:
                msg = str(n_state)
                while len(bytes(msg, "utf8")) < 16:
                    msg += "#"
                self.socket.sendall(bytes(msg, "utf8"))
                answer = self.socket.recv(Constants.ANSWER_BYTES_LENGTH)
                if str(answer, "utf8") == Constants.ANSWER_POSITIVE:
                    if n_state != self.state:
                        self.state = n_state
                        d = os.path.realpath('.')
                        now = datetime.datetime.now()
                        folder_name = str(now.day) + "_" + str(now.month) + "_" + str(now.year)
                        file = open(d + "\\log\\" + folder_name + "\\" + self.name + ".log", "a")
                        file.write(str(int(time.time())) + "_" + str(n_state) + "\n")
                        file.close()
                    return True
                else:
                    return False
            else:
                return False
        except:
            return False

    def check_connection(self):
        if self.switch_to(self.state):
            self.connected = True
            return True
        else:
            self.connected = False
            return False

    def get_state(self, as_string=False):
        if self.connected:
            if as_string:
                return self.state_names[self.state]
            else:
                return self.state
        else:
            if as_string:
                return "Disconnected"
            else:
                return -1
