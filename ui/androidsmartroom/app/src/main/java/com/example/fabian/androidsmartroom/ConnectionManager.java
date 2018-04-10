package com.example.fabian.androidsmartroom;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.nio.charset.StandardCharsets;

public class ConnectionManager {
    public static String send(String message) throws IOException {
        String serverName = "192.168.2.109";
        String str = "";
        int port = 2222;
		Socket client = new Socket(serverName, port);
		System.out.println("connected to " + client.getRemoteSocketAddress());

		OutputStream outToServer = client.getOutputStream();
		InputStream getFromServer = client.getInputStream();
		DataOutputStream out = new DataOutputStream(outToServer);
		DataInputStream input = new DataInputStream(getFromServer);

		String msg = message;

		while(msg.getBytes(StandardCharsets.UTF_8).length < 64) {
			msg += "#";
		}

		out.write(msg.getBytes(StandardCharsets.UTF_8));

		byte[] inputbyte = new byte[2048];
		input.read(inputbyte, 0, 2048);
		System.out.println(inputbyte);
		str = new String(inputbyte, StandardCharsets.UTF_8);

		client.close();
        return str;
    }

}

