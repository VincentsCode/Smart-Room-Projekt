import socket
import sys

from apscheduler.schedulers.background import BackgroundScheduler

import Constants

from PyQt5.QtCore import QObject, pyqtSignal
from PyQt5.QtWidgets import QWidget, QLabel, QComboBox, QApplication, QPushButton, QDialog, QLineEdit

ip = "localhost"
port = 2222
socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
socket.connect((ip, port))


# REQUEST INFO
def get_devices():
    msg = Constants.UI_CLIENT_DATA_REQUEST
    while len(bytes(msg, "utf8")) < Constants.REQUEST_LENGTH:
        msg += "#"
    socket.sendall(bytes(msg, "utf8"))
    answer = socket.recv(Constants.SERVER_ANSWER_LENGTH)
    s_a = str(answer, "utf8")
    return s_a


# EDIT COMPUTER
# noinspection PyShadowingNames,PyUnusedLocal
def edit_device(name, new_state):
    # create payload
    msg = Constants.UI_CLIENT_COMMAND_IDENTIFIER + str(name) + "_" + str(new_state)

    # send payload
    while len(bytes(msg, "utf8")) < Constants.REQUEST_LENGTH:
        msg += "#"
    socket.sendall(bytes(msg, "utf8"))
    answer = socket.recv(Constants.SERVER_ANSWER_LENGTH)


# ADD DEVICE
# noinspection PyUnusedLocal,PyDefaultArgument,PyShadowingNames
def add_device(name, ip, port, state_count=2, state_names=["AN", "AUS"]):
    # create payload
    msg = Constants.UI_CLIENT_ADD_DEVICE_IDENTIFIER
    msg += str(name) + "_"
    msg += str(ip) + "_"
    msg += str(port) + "_"
    msg += str(state_count) + "_"
    for s in state_names:
        msg += str(s) + "_"
    msg = msg[:-1]

    # send payload
    while len(bytes(msg, "utf8")) < Constants.REQUEST_LENGTH:
        msg += "#"
    socket.sendall(bytes(msg, "utf8"))
    answer = socket.recv(Constants.SERVER_ANSWER_LENGTH)


class Communicator(QObject):
    updateActorState = pyqtSignal(int)


# noinspection PyAttributeOutsideInit,PyUnresolvedReferences
class ActorWidget(QWidget):
    def __init__(self, x, y, name, ip, port, c_state, states, connected, lay):
        QWidget.__init__(self)
        self.name = name
        self.states = states
        self.c_state = c_state
        self.connected = connected

        self.label = QLabel(str(name) + " (" + str(ip) + ":" + str(port) + ")", lay)
        self.label.move(x, y)

        self.combo = QComboBox(lay)
        for state in states:
            self.combo.addItem(state)
        self.combo.move(x+160, y-2.5)
        self.combo.setFixedWidth(80)
        self.combo.currentTextChanged.connect(self.select)
        self.update_state(self.c_state, self.connected)
        if self.connected:
            self.combo.setEnabled(True)
        else:
            self.combo.setEnabled(False)
        self.label.updateMicroFocus()

    def select(self, val):
        val = self.states.index(val)
        edit_device(self.name, val)

    def update_state(self, n_state, n_connected):
        self.c_state = n_state
        self.combo.setCurrentIndex(n_state)

        self.connected = n_connected
        if n_connected:
            self.combo.setEnabled(True)
        else:
            self.combo.setEnabled(False)


# noinspection PyAttributeOutsideInit,PyShadowingNames
class MainWindow(QWidget):
    def __init__(self):
        QWidget.__init__(self)

        self.actors = {}

        req = get_devices()
        req = req.replace("#", "")
        s_req = req.split("+")
        idx = 1
        for s in s_req:
            s_s = s.split("_")
            name = str(s_s[0])
            ip = str(s_s[1])
            port = str(s_s[2])
            c_state = int(s_s[3])
            states = []
            for e in s_s[5].replace("'", "").replace("[", "").replace("]", "").split(", "):
                states.append(e)
            if s_s[6] == "False":
                connected = False
            else:
                connected = True

            self.actors[name] = ActorWidget(30, 25*idx, name, ip, port, c_state, states, connected, self)

            idx += 1

        class DevDiag(QDialog):
            def __init__(self):
                super(DevDiag, self).__init__()
                self.setWindowTitle("Gerät hinzufügen")
                self.setFixedWidth(260)
                self.labels = [
                    QLabel("Name: ", self),
                    QLabel("IP: ", self),
                    QLabel("Port: ", self),
                    QLabel("Stati: ", self)
                ]
                self.edits = [
                    QLineEdit(self),
                    QLineEdit(self),
                    QLineEdit(self),
                    QLineEdit(self)
                ]
                idx = 1
                for label in self.labels:
                    label.move(30, idx*25)
                    idx += 1
                idx = 1
                for edit in self.edits:
                    edit.move(80, idx*25-2.5)
                    edit.setFixedWidth(150)
                    idx += 1

                self.add_btn = QPushButton(self)
                self.add_btn.move(30, 25 * (len(self.edits) + 1))
                self.add_btn.setText("Gerät hinzufügen")
                self.add_btn.clicked.connect(self.add_it)

            def add_it(self):
                sn = self.edits[3].text().split(", ")
                add_device(self.edits[0].text(), self.edits[1].text(), self.edits[2].text(), len(sn), sn)
                for edit in self.edits: edit.setText("")
                self.close()

        form = DevDiag()

        def device_dialog():
            form.show()

        self.add_btn = QPushButton(self)
        self.add_btn.move(30, 25*(len(self.actors)+1))
        self.add_btn.setText("Gerät hinzufügen")
        self.add_btn.clicked.connect(device_dialog)

        def update_states():
            self.actors = {}

            req = get_devices()
            req = req.replace("#", "")
            s_req = req.split("+")
            idx = 1
            for s in s_req:
                s_s = s.split("_")
                name = str(s_s[0])
                ip = str(s_s[1])
                port = str(s_s[2])
                c_state = int(s_s[3])
                states = []
                for e in s_s[5].replace("'", "").replace("[", "").replace("]", "").split(", "):
                    states.append(e)
                if s_s[6] == "False":
                    connected = False
                else:
                    connected = True

                self.actors[name] = ActorWidget(30, 25 * idx, name, ip, port, c_state, states, connected, self)
                idx += 1

        t_delay = 1
        scheduler = BackgroundScheduler()
        scheduler.add_job(update_states, 'interval', seconds=t_delay)
        scheduler.start()

        self.setGeometry(300, 300, 304, (len(self.actors)+1)*25+40)
        self.setWindowTitle('Smart-Room')
        self.show()

    def on_activated(self, text):
        self.lbl.setText(text)
        self.lbl.adjustSize()


if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = MainWindow()
    sys.exit(app.exec_())