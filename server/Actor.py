import socket
import Constants


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
    def __init__(self, m_ip, m_port, state_count, state_names=None):
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

    def __repr__(self):
        return "<Actor.AdvancedSwitch object: ip={}, port={}, connected={}, state={}, state_count={}, state_names={}>"\
            .format(self.ip, self.port, self.connected, self.state, self.state_count, self.state_names)

    def connect(self):
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.socket.connect((self.ip, self.port))
        self.connected = True

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
        if self.connected and n_state <= self.state_count:
            msg = str(n_state)
            while len(bytes(msg, "utf8")) < 16:
                msg += "#"
            self.socket.sendall(bytes(msg, "utf8"))
            answer = self.socket.recv(Constants.ANSWER_BYTES_LENGTH)
            if str(answer, "utf8") == Constants.ANSWER_POSITIVE:
                self.state = n_state
                return True
            else:
                return False
        else:
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
