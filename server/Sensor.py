# ABSTRACT SENSOR - DO NOT INSTANTIATE
class Sensor:
    def __init__(self, m_ip):
        self.IP = m_ip
        self.initialize_sensor()

    def get_ip(self):
        return self.IP

    def initialize_sensor(self):
        pass

    def get_data(self):
        pass


# BASIC CAMERA
class Camera(Sensor):
    pass


# BASIC LIGHT-SENSOR
class Light(Sensor):
    pass


# BASIC TEMPERATURE-SENSOR
class Temperature(Sensor):
    pass


# BASIC MICROPHONE
class Microphone(Sensor):
    pass

