using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;

  
[RequireComponent (typeof(AudioSource))]  

public class MicrophoneManager : MonoBehaviour
{

	private static MicrophoneManager _instance;

	public static MicrophoneManager getInstance ()
	{  
		if (_instance == null) {  
			_instance = GameManager.getInstance ().Stage.AddComponent<MicrophoneManager> ();  
		}  
		return _instance;  
	}


	//	public float sensitivity=100;
	//	public float loudness=0;
	const int F2IFacter = 32767;
	//to convert float to Int16
	private AudioSource inputAudio;
	private AudioSource outputAudio;

	private static string[] devices = null;

	const int HEADER_SIZE = 44;
	const int RECORD_TIME = 10;
	List<int> _uuidList;
	private AudioClip redioclip;
	private bool _isAvalable = true;
	// Use this for initialization
	public void init ()
	{  
		devices = Microphone.devices;  
		_isAvalable = devices.Length > 0;
		if (!_isAvalable) {  
			Debug.LogError ("Microphone.devices is null");  
		}
		foreach (string name in devices) {  
			Debug.Log ("device name = " + name);  
		}  
		SocketEventHandle.getInstance ().micInputNotice += onMicInputNotify;
		inputAudio = GameObject.Find ("GamePlayAudio").GetComponent<AudioSource> ();
		if (inputAudio.clip == null) {
			inputAudio.clip = AudioClip.Create ("playRecordClip", 160000, 1, 8000, false);  
		}
		outputAudio = GetComponent<AudioSource> ();
	}

	public void StartRecord (List<int> uuidList)
	{  
		if (_isAvalable) {
			_uuidList = uuidList;
			outputAudio.Stop ();  
		   
			redioclip = Microphone.Start ("inputMicro", false, RECORD_TIME, 8000); //22050    

			Debug.Log ("StartRecord");  
		}
	}

	public  void StopRecord ()
	{  
		if (_isAvalable) {
			Debug.Log ("StopRecord");
			if (!Microphone.IsRecording (null) || redioclip == null) {
				Debug.Log ("未录有声音");
				return;  
			}  
			Microphone.End (null);  

			float[] samples = new float[redioclip.samples];  
			redioclip.GetData (samples, 0);  
			Byte[] outData = new byte[samples.Length * 2];  
			for (int i = 0; i < samples.Length; i++) {  
				short temshort = (short)(samples [i] * F2IFacter);  
				Byte[] temdata = BitConverter.GetBytes (temshort);  
				outData [i * 2] = temdata [0];  
				outData [i * 2 + 1] = temdata [1];  
			}  
			ChatSocket.getInstance ().sendMsg (new MicInputRequest (_uuidList, outData));

			outputAudio.clip = redioclip;
			outputAudio.mute = false;  
			outputAudio.loop = false;  
			outputAudio.Play ();  
		}
	}


	private  float GetAveragedVolume ()
	{  
		float[] data = new float[256];  
		float a = 0;  
		outputAudio.GetOutputData (data, 0);  
		foreach (float s in data) {  
			a += Mathf.Abs (s);  
		}  
		return a / 256;  
	}

	private void onMicInputNotify (ClientResponse response)
	{
		if (GlobalData.getInstance ().SoundToggle) {
			byte[] data = response.bytes;
			int i = 0;
			List<short> result = new List<short> ();
			while (data.Length - i >= 2) {
				result.Add (BitConverter.ToInt16 (data, i));
				i += 2;
			}
			Int16[] inData = result.ToArray ();
			if (inData.Length > 0) {  
				float[] samples = new float[inData.Length];  
				for (i = 0; i < inData.Length; i++) {  
					samples [i] = (float)inData [i] / F2IFacter;  
				}  

				inputAudio.clip.SetData (samples, 0);  
				inputAudio.mute = false;  
				inputAudio.Play ();  
			}  

		}
	}
	//save to localhost
	public bool Save (string filename)
	{  
		
		AudioClip clip = outputAudio.clip;

		if (!filename.ToLower ().EndsWith (".wav")) {  
			filename += ".wav";  
		}  

		string filepath = Path.Combine (Application.persistentDataPath, filename);  

		Debug.Log ("save to:"+filepath);  

		// Make sure directory exists if user is saving to sub dir.  
		if (!Directory.Exists (Path.GetDirectoryName (filepath)))
			Directory.CreateDirectory (Path.GetDirectoryName (filepath));

		using (FileStream fileStream = CreateEmpty (filepath)) {
			ConvertAndWrite (fileStream, clip);
			WriteHeader (fileStream, clip);
		}  

		return true; // TODO: return false if there's a failure saving the file  
	}

	private FileStream CreateEmpty (string filepath)
	{  
		FileStream fileStream = new FileStream (filepath, FileMode.Create);  
		byte emptyByte = new byte ();  

		for (int i = 0; i < HEADER_SIZE; i++) { //preparing the header  
			fileStream.WriteByte (emptyByte);  
		}  

		return fileStream;  
	}

	private void ConvertAndWrite (FileStream fileStream, AudioClip clip)
	{  

		float[] samples = new float[clip.samples];  

		clip.GetData (samples, 0);  

		Int16[] intData = new Int16[samples.Length];  

		Byte[] bytesData = new Byte[samples.Length * 2];  

		for (int i = 0; i < samples.Length; i++) {  
			intData [i] = (short)(samples [i] * F2IFacter);  
			Byte[] byteArr = new Byte[2];  
			byteArr = BitConverter.GetBytes (intData [i]);  
			byteArr.CopyTo (bytesData, i * 2);  
		}  

		fileStream.Write (bytesData, 0, bytesData.Length);  
	}

	private void WriteHeader (FileStream fileStream, AudioClip clip)
	{  

		int hz = clip.frequency;  
		int channels = clip.channels;  
		int samples = clip.samples;  

		fileStream.Seek (0, SeekOrigin.Begin);  

		Byte[] riff = System.Text.Encoding.UTF8.GetBytes ("RIFF");  
		fileStream.Write (riff, 0, 4);  

		Byte[] chunkSize = BitConverter.GetBytes (fileStream.Length - 8);  
		fileStream.Write (chunkSize, 0, 4);  

		Byte[] wave = System.Text.Encoding.UTF8.GetBytes ("WAVE");  
		fileStream.Write (wave, 0, 4);  

		Byte[] fmt = System.Text.Encoding.UTF8.GetBytes ("fmt ");  
		fileStream.Write (fmt, 0, 4);  

		Byte[] subChunk1 = BitConverter.GetBytes (16);  
		fileStream.Write (subChunk1, 0, 4);  

//		UInt16 two = 2;  
		UInt16 one = 1;  

		Byte[] audioFormat = BitConverter.GetBytes (one);  
		fileStream.Write (audioFormat, 0, 2);  

		Byte[] numChannels = BitConverter.GetBytes (channels);  
		fileStream.Write (numChannels, 0, 2);  

		Byte[] sampleRate = BitConverter.GetBytes (hz);  
		fileStream.Write (sampleRate, 0, 4);  

		Byte[] byteRate = BitConverter.GetBytes (hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2  
		fileStream.Write (byteRate, 0, 4);  

		UInt16 blockAlign = (ushort)(channels * 2);  
		fileStream.Write (BitConverter.GetBytes (blockAlign), 0, 2);  

		UInt16 bps = 16;  
		Byte[] bitsPerSample = BitConverter.GetBytes (bps);  
		fileStream.Write (bitsPerSample, 0, 2);  

		Byte[] datastring = System.Text.Encoding.UTF8.GetBytes ("data");  
		fileStream.Write (datastring, 0, 4);  

		Byte[] subChunk2 = BitConverter.GetBytes (samples * channels * 2);  
		fileStream.Write (subChunk2, 0, 4);  

		//      fileStream.Close();  
	}
}