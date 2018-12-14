using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poker : MonoBehaviour {

	private enum Status 
	{ 
		Moving = 0, 
		Smooth = 1, 
		End = 2,
	};

	public float pokerWidth;
	public float pokerHeight;
	public float pokerDiv;
	public float pokerRadius;
	public float touchPercent;
	public float RotationFrame;
	public float RotationAnger;
	public float SmoothFrame;
	public float SmoothAnger;
	private Status curStatus;
	private int actionFrame;
	// Use this for initialization
	void Start () {
        actionFrame = 0;
        RotationFrame = 50.0f;
        RotationAnger = 3.1415926f/3.0f;
        SmoothFrame = 50.0f;
        SmoothAnger = 3.1415926f/4.0f;
        curStatus = Status.Moving;
        pokerWidth = 2.0f;
        pokerHeight = 2.0f;
        pokerDiv = 30.0f;
        pokerRadius = pokerHeight/10.0f;
        float pointNum = pokerDiv*2.0f + 2.0f;
 		Vector3[] newVertices = new Vector3[(int)pointNum];
 		float heightDis = pokerHeight/pokerDiv;
 		float startPosX = -pokerWidth/2.0f;
 		float startPosY = -pokerHeight/2.0f;

 		for (int i = 0; i < pointNum; i++)
		{
			float posX = startPosX;
			float posY = startPosY;
			float posZ = 0.0f;
			if(i%2 == 0){
				posY = (i/2.0f)*heightDis;
			}else{
				posX = startPosX+pokerHeight;
				posY = ((i-1.0f)/2.0f)*heightDis;
			}
			posY= posY + startPosY;
			newVertices[i] = new Vector3(posX, posY, posZ);
		}

 		Vector2[] newUV = new Vector2[(int)pointNum];
 		heightDis = 1.0f/pokerDiv;
 		for (int i = 0; i < pointNum; i++)
		{
			if(i%2 == 0){
				newUV[i] = new Vector3(0.0f, (i/2.0f)*heightDis, 0.0f);
			}else{
				newUV[i] = new Vector3(1.0f, ((i-1.0f)/2.0f)*heightDis, 0.0f);
			}
		}

 		int triangleNum = (int)pointNum*3;
 		int[] newTriangles = new int[triangleNum];
 		int index = 0;
 		for (int i = 0; i < ((int)pointNum-2); i=i+2){
 			newTriangles[index++] = i;
 			newTriangles[index++] = i+1;
 			newTriangles[index++] = i+2;
 			newTriangles[index++] = i+1;
 			newTriangles[index++] = i+3;
 			newTriangles[index++] = i+2;
 		}

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
 
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
	}
	
	void calMoveVertices (float rotation) {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] newVertices = mesh.vertices;
		float pointNum = pokerDiv*2.0f + 2.0f;
 		float heightDis = pokerHeight/pokerDiv;
 		float startPosX = -pokerWidth/2.0f;
 		float startPosY = -pokerHeight/2.0f;
		float halfPeri = pokerRadius * 3.14159f;
		float hr = pokerHeight * touchPercent;

 		for (int i = 0; i < pointNum; i++)
		{
			float posX = startPosX;
			float posY = startPosY;
			float posZ = 0.0f;
			if(i%2 == 0){
				posY = (i/2.0f)*heightDis;
			}else{
				posX = startPosX+pokerHeight;
				posY = ((i-1.0f)/2.0f)*heightDis;
			}
			if(hr > 0.0f && hr <= halfPeri){
				if(posY < hr){
					float rad = hr/ 3.14159f;
					float arc = (hr-posY)/rad;
					posY = hr - Mathf.Sin(arc)*rad;
					posZ = rad * (1.0f- Mathf.Cos(arc));
				}
			}
			if(hr > halfPeri){
				float straight = (hr - halfPeri)/2.0f;
				if(posY < straight){
					posY = hr  - posY;
					posZ = pokerRadius * 2.0f;
				}else if(posY < (straight + halfPeri)) {
					float dy = halfPeri - (posY - straight);
					float arc = dy/pokerRadius;
					posY = hr - straight - Mathf.Sin(arc)*pokerRadius;
					posZ = pokerRadius * (1.0f- Mathf.Cos(arc));
				}
			}

			float y1 = posY;
			float z1 = posZ;
			float y2 = pokerHeight;
			float z2 = 0.0f;
			float sinRat = Mathf.Sin(rotation);
			float cosRat = Mathf.Cos(rotation);
			posY=(y1-y2)*cosRat-(z1-z2)*sinRat+y2;
			posZ=(z1-z2)*cosRat+(y1-y2)*sinRat+z2;
			posY = posY - pokerHeight/2.0f*(1.0f-cosRat);

			posY= posY + startPosY;
			newVertices[i].x = posX;
			newVertices[i].y = posY;
			newVertices[i].z = posZ;
		}
		mesh.vertices = newVertices;
	}

	void calSmoothVertices (float rotation) {		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] newVertices = mesh.vertices;
		float pointNum = pokerDiv*2.0f + 2.0f;
 		float heightDis = pokerHeight/pokerDiv;
 		float startPosX = -pokerWidth/2.0f;
 		float startPosY = -pokerHeight/2.0f;
		float cl = pokerHeight/5.0f;
		float sl = (pokerHeight - cl)/2.0f;
		float radii = (cl/rotation)/2.0f;
		float sinRot = Mathf.Sin(rotation);
		float cosRot = Mathf.Cos(rotation);
		float distance = radii*sinRot;
		float centerY = pokerHeight/2.0f;
		float poxY1 = centerY - distance;
		float poxY2 = centerY + distance;
		float posZ1 = sl*sinRot;

 		for (int i = 0; i < pointNum; i++)
		{
			float posX = startPosX;
			float posY = startPosY;
			float posZ = 0.0f;
			if(i%2 == 0){
				posY = (i/2.0f)*heightDis;
			}else{
				posX = startPosX+pokerHeight;
				posY = ((i-1.0f)/2.0f)*heightDis;
			}
			if(posY <= sl){
				float length = sl - posY;
				posY = poxY1 - length*cosRot;
				posZ = posZ1 - length*sinRot;
			}else if(posY < (sl+cl)){
				float el = posY - sl;
				float rotation2 = -el/radii;
				float x1 = poxY1;
				float y1 = posZ1;
				float x2 = centerY;
				float y2 = posZ1 - radii*cosRot;
				float sinRot2 = Mathf.Sin(rotation2);
				float cosRot2 = Mathf.Cos(rotation2);
				posY=(x1-x2)*cosRot2-(y1-y2)*sinRot2+x2;
				posZ=(y1-y2)*cosRot2+(x1-x2)*sinRot2+y2;
			}else if(posY <= pokerHeight){
				float length = posY - cl - sl;
				posY = poxY2 + length*cosRot;
				posZ = posZ1 - length*sinRot;
			}
			posY= posY + startPosY;
			newVertices[i].x = posX;
			newVertices[i].y = posY;
			newVertices[i].z = posZ;
		}
		mesh.vertices = newVertices;
	}

	// Update is called once per frame
	void Update () {
		Debug.Log("Input.touchCount="+Input.touchCount);
		if (Input.touchCount > 0) {
			if(Input.GetTouch(0).phase == TouchPhase.Began){
				Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
				touchPercent = touchDeltaPosition.y / Screen.height;
				Debug.Log("Began Screen.height="+Screen.height);
				Debug.Log("Began touchDeltaPosition.y="+touchDeltaPosition.y);
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Moved){
				Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
				touchPercent = touchDeltaPosition.y / Screen.height;
				Debug.Log("Moved Screen.height="+Screen.height);
				Debug.Log("Moved touchDeltaPosition.y="+touchDeltaPosition.y);
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled){
				touchPercent = 0;
			}
        }
        touchPercent = 0.5f;
		
		actionFrame = actionFrame + 1;
		if(actionFrame <= RotationFrame){
			calMoveVertices (-RotationAnger*actionFrame/RotationFrame);
		}else if(actionFrame < (RotationFrame+SmoothFrame)){
			float scale = (actionFrame - RotationFrame)/SmoothFrame;
			float rotation = Mathf.Max(0.01f,SmoothAnger*(1.0f-scale));
			calSmoothVertices(rotation);
		}else{
			calMoveVertices(0.0f);
		}
	}
}
