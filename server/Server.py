import json
import os

import Actor

c_dir = os.path.realpath('.')
actors_file_name = c_dir + '\\data\\actors.json'

if __name__ == "__main__":
    actors_file = open(actors_file_name, 'r')
    actors_list = json.load(actors_file)
    actors_file.close()

    print('actors:', actors_list)
    print('actor_count:', len(actors_list))
    a = Actor.AdvancedSwitch("192.168.2.1", 2, ["An", "Aus"])
