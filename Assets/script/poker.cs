using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poker : MonoBehaviour {

	public float pokerWidth;
	public float pokerHeight;
	public float pokerDiv;
	public float pokerRadius;
	public float touchPercent;
	// Use this for initialization
	void Start () {
		/*Vector3[] newVertices = { new Vector3(-1, -1, 0), new Vector3(-1, 1, 0), new Vector3(1, 1, 0), new Vector3(1, -1, 0) };
        Vector2[] newUV = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
        int[] newTriangles = {0,2,1,0,3,2};*/
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
	
	void changeVertices () {
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
		changeVertices ();
	}
}
