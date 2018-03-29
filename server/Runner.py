import json
import os

import Sensor
import Actor

c_dir = os.path.realpath('.')
actors_file_name = c_dir + '\\data\\actors.json'
sensors_file_name = c_dir + '\\data\\sensors.json'

if __name__ == "__main__":
    actors_file = open(actors_file_name, 'r')
    actors_list = json.load(actors_file)
    actors_file.close()

    sensors_file = open(sensors_file_name, 'r')
    sensors_list = json.load(sensors_file)
    sensors_file.close()

    print('actors:', actors_list)
    print('actor_count:', len(actors_list))

    print('sensors:', sensors_list)
    print('sensor_count:', len(sensors_list))

    actors = []
    for actor_name in actors_list:
        actor_properties = actors_list[actor_name]
        actor_type = actor_properties['type']
        actor_ip = actor_properties['ip']

        if actor_type == 'Heater':
            actors.append(Actor.Heater(actor_ip))
        if actor_type == 'Light':
            actors.append(Actor.Light(actor_ip))
        if actor_type == 'Computer':
            actors.append(Actor.Computer(actor_ip))

    sensors = []
    for sensor_name in sensors_list:
        sensor_properties = sensors_list[sensor_name]
        sensor_type = sensor_properties['type']
        sensor_ip = sensor_properties['ip']

        if sensor_type == 'Camera':
            sensors.append(Sensor.Camera(sensor_ip))
        if sensor_type == 'Light':
            sensors.append(Sensor.Light(sensor_ip))
        if sensor_type == 'Temperature':
            sensors.append(Sensor.Temperature(sensor_ip))
        if sensor_type == 'Microphone':
            sensors.append(Sensor.Microphone(sensor_ip))

    print(sensors)