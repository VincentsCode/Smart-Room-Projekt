package com.example.fabian.androidsmartroom;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.design.widget.FloatingActionButton;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;

public class Menu_Devices extends Fragment{


    public void newDevice() {
        Intent i = new Intent(getContext(), AddDevice.class);
        startActivity(i);
    }


    public void create(View view) {

        ConnectionDetector cd = new ConnectionDetector(getActivity());

        String avail = cd.isInternetOn();
        String answer;

        if (avail.equals("online")) {
            answer = DataProcess.sendDataRequest();

            ArrayList<DataModel> dataModels;
            ListView listView = view.findViewById(R.id.list);
            dataModels = new ArrayList<>();
            // TODO IF
            String[] devices = answer.split("\\+");
            for (String device : devices) {
                String[] deviceInfo = device.split("_");
                String availability = "";
                if (deviceInfo[6].contains("True")) {
                    availability = "online";
                }
                else {
                    availability = "offline";
                }
                dataModels.add(new DataModel(deviceInfo[0], deviceInfo[1], deviceInfo[2], deviceInfo[3], deviceInfo[4], deviceInfo[5], availability));
            }

            CustomAdapter adapter = new CustomAdapter(dataModels, getContext());
            listView.setAdapter(adapter);
        }
        else {
            Toast.makeText(getActivity(), "Bitte schalten sie ihr WLAN an", Toast.LENGTH_SHORT).show();
        }

    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, Bundle savedInstanceState) {
        final View view = inflater.inflate(R.layout.devices, container, false);
        return view;
    }

    @Override
    public void onViewCreated(View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        create(view);
        getActivity().setTitle("Geräte");
        FloatingActionButton btn = view.findViewById(R.id.fab);
        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                newDevice();
            }
        });
        final View viewCopy = view;

        Timer timer = new Timer();
        timer.scheduleAtFixedRate(new TimerTask() {
            @Override
            public void run() {
                getActivity().runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        create(viewCopy);
                    }
                });
            }
        }, 1000, 1000);

    }



}
