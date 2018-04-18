package com.example.fabian.androidsmartroom;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;
import android.widget.Toolbar;

import java.util.Objects;

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

        if (!Objects.equals(nameText, "") && !Objects.equals(ipText, "") && !Objects.equals(portText, "") && !Objects.equals(statesText, "")) {
            String[] stateNames = statesText.split(",");
            int stateCount = stateNames.length;

            String res = "ADD_" + nameText + "_" + ipText + "_" + portText + "_" + stateCount;
            for (String n : stateNames) {
                res += "_" + n;
            }
            backToDevices();
            ConnectionManager.send(res);
            return res;
        }
        else {
            Toast.makeText(this, "Bitte füllen Sie alle Felder aus", Toast.LENGTH_SHORT).show();
        }
        return null;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_device);
        setTitle("Gerät hinzufügen");
        Button addBtn = findViewById(R.id.button3);

        Toolbar toolbar = findViewById(R.id.toolbar);
        setActionBar(toolbar);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        Button addDevice = findViewById(R.id.button3);
        addDevice.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                getData();
            }
        });
    }


    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

}
