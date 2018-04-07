package com.example.fabian.androidsmartroom;


public class DataModel {

    private String name;
    private String type;
    private String version_number;
    private String feature;

    DataModel(String name, String type, String version_number, String feature) {
        this.name=name;
        this.type=type;
        this.version_number=version_number;
    this.feature=feature;

    }

    public String getName() {
        return name;
    }

    String getType() {
        return type;
    }

    String getVersion_number() {
        return version_number;
    }

    String getFeature() {
        return feature;
    }
}