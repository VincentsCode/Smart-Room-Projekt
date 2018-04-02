package com.example.fabian.jufo2019;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.SeekBar;
import android.widget.TextView;

public class heaterActivity extends AppCompatActivity {

    ;
    SeekBar seekBar;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        final TextView temp;
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_heater);
        temp = (TextView) findViewById(R.id.textView5);
        seekBar = (SeekBar) findViewById(R.id.seekBarHeater);
        seekBar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onProgressChanged(SeekBar seekBar, int i, boolean b) {
                double t = i/4 + 15;
                if (t < 40 && t != 15) {
                    temp.setText("Raumtemperatur auf " + t + "°C gesetzt");
                }
                else if (t == 40) {
                    temp.setText("Raumtemperatur auf maximale Temperatur gesetzt (40 °C)");
                }
                else {
                    temp.setText("Raumtermperatur auf minmale Temperatur gesetzt (15 °C)");
                }
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {

            }

            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {

            }
        });


    }

    public void openMain(View view) {
        Intent i = new Intent(this, MainActivity.class);
        startActivity(i);
    }


}
