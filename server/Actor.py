# ABSTRACT ACTOR - DO NOT INSTANTIATE
class Actor:
    def __init__(self, m_ip):
        self.IP = m_ip

    # IMPLEMENT ACTIONS


# BASIC HEATER
class Heater(Actor):
    # TODO
    # Intelligent Heat-Regulator?
    def __init__(self, m_ip):
        super().__init__(m_ip)


# ABSTRACT SWITCH - DO NOT INSTANTIATE
class Switch(Actor):
    def __init__(self, m_ip):
        super().__init__(m_ip)
        self.on = False

    def switch(self):
        # IMPLEMENT
        self.on = not self.on

    def turn_off(self):
        if self.on:
            self.switch()
            self.on = False

    def turn_on(self):
        if not self.on:
            self.switch()
            self.on = True

    def is_on(self):
        return self.on

    def is_off(self):
        return not self.on


# BASIC LIGHT
class Light(Switch):
    def switch(self):
        # TODO
        # SEND IR-BEAM
        pass


# BASIC COMPUTER
class Computer(Switch):
    def switch(self):
        # TODO
        # PUT RPi-Zero W o.ä. in Pwr-Btn-Line || WAKE ON LAN (WOL)
        pass


# DIGITAL CLASSES

# DIGITAL-ACTOR
class DActor(Actor):
    # TODO
    pass

    
# DIGITAL-SWITCH
class DSwitch(Switch):
    def switch(self):
        # TODO
        pass

