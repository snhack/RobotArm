﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using SnhackRobotArm.Properties;

namespace SnhackRobotArm
{
	class ServoControllerBoard
	{
		SerialPort port;
		DateTime lastServoCommand = DateTime.Now;

		public ServoControllerBoard()
		{
			try
			{
				port = new SerialPort(Settings.Default.ComPort, 115200, Parity.None, 8, StopBits.One);
				port.Open();
			}
			catch (Exception)
			{
				Console.WriteLine("Servo Controller Board not connected, no commands will be sent.");
			}
		}

		public void SendServoUpdates(IEnumerable<Servo> servos, bool force)
		{
			if ((DateTime.Now - lastServoCommand).TotalMilliseconds > 50 || force)
			{
				var command = new StringBuilder();
				foreach(var servo in servos)
					command.Append(servo.UpdateCommand());

				var commandStr = command.ToString();
				Console.WriteLine(commandStr);
				if (port.IsOpen)
				{
					port.Write(commandStr);
					port.Write("\r");
				}

				lastServoCommand = DateTime.Now;
			}
		}
	}
}
