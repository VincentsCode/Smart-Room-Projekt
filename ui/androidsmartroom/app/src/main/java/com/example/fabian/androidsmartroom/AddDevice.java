package com.example.fabian.androidsmartroom;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

public class AddDevice extends AppCompatActivity {

    public void backToDevices() {

        finish();
    }

    public String getData() {
        EditText name = findViewById(R.id.name);
        EditText ip = findViewById(R.id.ip);
        EditText port = findViewById(R.id.port);
        EditText states = findViewById(R.id.stadien);
        String nameText = name.getText().toString();
        String ipText = ip.getText().toString();
        String portText = port.getText().toString();
        String statesText = states.getText().toString();

        String[] stateNames = statesText.split(",");
        int stateCount = stateNames.length;

        String res = "ADD_" + nameText + "_" + ipText + "_" + portText + "_" + stateCount;
        for (String n : stateNames) {
            res += "_" + n;
        }
        return res;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_device);
        setTitle("Gerät hinzufügen");
        Button addBtn = findViewById(R.id.button3);

        addBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                ConnectionManager.send(getData());
                Toast.makeText(AddDevice.this, "Gerät hinzugefügt", Toast.LENGTH_SHORT).show();
                backToDevices();
            }
        });

    }
}
