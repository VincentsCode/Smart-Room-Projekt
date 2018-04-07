package com.example.fabian.androidsmartroom;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;

public class Menu_Devices extends Fragment{

    public void client(String a, int b) {
        String serverName = "192.168.2.109";
        int port = 2222;
        try {
            Socket client = new Socket(serverName, port);

            Toast.makeText(getActivity(), "erfolgreich zu Server verbunden", Toast.LENGTH_SHORT).show();

            OutputStream outToServer = client.getOutputStream();
            InputStream getFromServer = client.getInputStream();
            DataOutputStream out = new DataOutputStream(outToServer);
            DataInputStream input = new DataInputStream(getFromServer);

            String msg = Constants.UI_CLIENT_COMMAND_IDENTIFIER + a + "_" + b;

            while(msg.getBytes(StandardCharsets.UTF_8).length < 32) {
                msg += "#";
            }

            out.write(msg.getBytes(StandardCharsets.UTF_8));

            byte[] inputbyte = new byte[2048];
            input.read(inputbyte, 0, 2048);
            System.out.println(inputbyte);
            String str = new String(inputbyte, StandardCharsets.UTF_8);
            System.out.println(str);

            client.close();
        } catch (IOException e1) {
            e1.printStackTrace();
        }
    }


    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, Bundle savedInstanceState) {

        View view = inflater.inflate(R.layout.devices, container, false);
        ArrayList<DataModel> dataModels;
        ListView listView = view.findViewById(R.id.list);

        dataModels = new ArrayList<>();

        dataModels.add(new DataModel("Apple Pie", "Android 1.0", "1","September 23, 2008"));
        dataModels.add(new DataModel("Banana Bread", "Android 1.1", "2","February 9, 2009"));
        dataModels.add(new DataModel("Cupcake", "Android 1.5", "3","April 27, 2009"));


        CustomAdapter adapter = new CustomAdapter(dataModels, getContext());

        listView.setAdapter(adapter);

        return view;
    }

    @Override
    public void onViewCreated(View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        getActivity().setTitle("Geräte");
    }
    public void addListItem(int device) {

    }

}
