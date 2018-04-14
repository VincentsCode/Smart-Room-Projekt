package com.example.fabian.androidsmartroom;

import android.content.Context;
import android.support.annotation.NonNull;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.TextView;

import java.util.ArrayList;

public class CustomAdapter extends ArrayAdapter<DataModel> implements View.OnClickListener{

    // View lookup cache
    private static class ViewHolder {
        TextView txtName;
        Button info;
    }

    CustomAdapter(ArrayList<DataModel> data, Context context) {
        super(context, R.layout.row_item, data);
    }

    @Override
    public void onClick(View v) {
        int position=(Integer) v.getTag();
        Object object= getItem(position);
        DataModel dataModel=(DataModel)object;

        switch (v.getId()) {
            case R.id.item_info:
                assert dataModel != null;
                String state = dataModel.getCurrentStateAsInt();
                int stateAsInt = Integer.valueOf(state);
                int maxState = Integer.valueOf(dataModel.getStatesCount()) - 1;
                String name = dataModel.getName();
                DevicesManager devicesManager = new DevicesManager(stateAsInt, maxState, name);
                devicesManager.switchState();
        }
    }

    @NonNull
    @Override
    public View getView(int position, View convertView, @NonNull ViewGroup parent) {

        DataModel dataModel = getItem(position);

        ViewHolder viewHolder;

        if (convertView == null) {

            viewHolder = new ViewHolder();
            LayoutInflater inflater = LayoutInflater.from(getContext());
            convertView = inflater.inflate(R.layout.row_item, parent, false);
            viewHolder.txtName = convertView.findViewById(R.id.name);
            viewHolder.info = convertView.findViewById(R.id.item_info);

            convertView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) convertView.getTag();
        }
        assert dataModel != null;
        if (dataModel.getAvailability().equals("online")) {
            viewHolder.txtName.setText(dataModel.getName() + " (" + dataModel.getAvailability() + ")");
            viewHolder.info.setOnClickListener(this);
            viewHolder.info.setTag(position);
            viewHolder.info.setText(dataModel.getCurrentState());
            viewHolder.info.setEnabled(true);
        }
        else {
            viewHolder.txtName.setText(dataModel.getName() + " (" + dataModel.getAvailability() + ")");
            viewHolder.info.setOnClickListener(this);
            viewHolder.info.setTag(position);
            viewHolder.info.setText("offline");
            viewHolder.info.setEnabled(false);
        }




        return convertView;
    }
}