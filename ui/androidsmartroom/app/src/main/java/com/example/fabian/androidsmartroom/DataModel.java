package com.example.fabian.androidsmartroom;

public class DataModel {

    private String name;
    private String ip;
    private String port;
    private String currentState;
    private String statesCount;
    private String states;
    private String availability;

    DataModel(String name, String ip, String port, String currentState, String statesCount, String states, String availability) {
        this.name = name;
        this.ip = ip;
        this.port = port;
        this.currentState = currentState;
        this.statesCount = statesCount;
        this.states = states;
        this.availability = availability;

    }

    public String getName() {
        return name;
    }

    String getIP() {
        return ip;
    }

    String getPort() { return port; }

    String getCurrentState() {
        String states = getStates();
        states = states.replace("[", "");
        states = states.replace("]", "");
        states = states.replace("'", "");
        String[] statesArray = states.split(",");
        return statesArray[Integer.valueOf(currentState)];
    }

    String getStatesCount() {
        return statesCount;
    }

    String getStates() {
        return states;
    }

    String getAvailability() {
        return availability;
    }

    String getCurrentStateAsInt() {
        return currentState;
    }
}