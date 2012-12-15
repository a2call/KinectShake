package com.example.test;

import java.io.*;
import java.net.*;

import android.content.Context;
import android.os.Vibrator;
import android.widget.EditText;
import android.widget.TextView;


public class SocketClient implements Runnable{
	String outText;
	MainActivity mactivity;
	public SocketClient(String text, MainActivity mac)
	{
		outText=text;
		mactivity=mac;
	}
	public SocketClient(String text)
	{
		outText=text;
	}
	public void run() {
		// TODO Auto-generated method stub
		Socket socket = null;
		DataOutputStream dataOutputStream = null;
		BufferedReader dataInputStream = null;

		try {
			socket = new Socket("10.109.171.228", 8888);
			dataOutputStream = new DataOutputStream(socket.getOutputStream());
			//dataInputStream = new DataInputStream(socket.getInputStream());
			dataOutputStream.writeBytes(outText);
			
			dataInputStream= new BufferedReader(new InputStreamReader(socket.getInputStream()));
			String in = dataInputStream.readLine();
			
			if(in.equals("repel"))
			{
				mactivity.vibrate(1000);
			}
			if(in.equals("attract"))
			{
				try
				{	mactivity.vibrate(100);
					
					Thread.sleep(100);
					mactivity.vibrate(100);
					Thread.sleep(100);
					mactivity.vibrate(100);
				}
				catch(Exception e)
				{
					
				}
			}
			//ok
			//attract
			//repel
			System.out.println(in);
			
		} catch (UnknownHostException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} finally {
			if (socket != null) {
				try {
					socket.close();
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}

			if (dataOutputStream != null) {
				try {
					dataOutputStream.close();
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}

			if (dataInputStream != null) {
				try {
					dataInputStream.close();
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
	}
	public static void main (String [] args){
		Thread t = new Thread(new SocketClient("hello"));
		t.start();
	}
};