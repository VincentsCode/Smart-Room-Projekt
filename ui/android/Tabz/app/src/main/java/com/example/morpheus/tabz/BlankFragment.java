package com.example.morpheus.tabz;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.SeekBar;
import android.widget.Switch;
import android.widget.TextView;

public class BlankFragment extends Fragment {
    public static final String ARG = "parameter";


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        View root = inflater.inflate(R.layout.fragment_blank, container, false);
        Bundle args = getArguments();
        String param = Integer.toString(args.getInt(BlankFragment.ARG));
        if (param.contains("1")) {
            ((TextView)root.findViewById(R.id.text)).setText("Heizung:");
            ((TextView)root.findViewById(R.id.textView)).setText("ist an");
            ((Switch)root.findViewById(R.id.switch1)).setVisibility(View.INVISIBLE);

        }
        else if (param.contains("2")) {
            ((TextView)root.findViewById(R.id.text)).setText("Licht:");
            ((TextView)root.findViewById(R.id.textView)).setText("ist manchmal an");
            ((SeekBar)root.findViewById(R.id.seekbar)).setVisibility(View.INVISIBLE);
            ((Switch)root.findViewById(R.id.switch1)).setText("AN - AUS");

        }
        else {
            ((TextView)root.findViewById(R.id.text)).setText("Computer:");
            ((TextView)root.findViewById(R.id.textView)).setText("ist aus");
            ((SeekBar)root.findViewById(R.id.seekbar)).setVisibility(View.INVISIBLE);
            ((Switch)root.findViewById(R.id.switch1)).setText("AN - AUS");

        }
        return root;
    }

}
