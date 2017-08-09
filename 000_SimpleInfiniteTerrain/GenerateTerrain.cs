//GenerateTerrain.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
	//to be called by a SendMessage event
	void ResizeTo (int edgeSizeInQuads)
	{
		int edgeSizeInVertices = edgeSizeInQuads + 1; //avoid fencepost error
		Vector3[] vertices = new Vector3[edgeSizeInVertices * edgeSizeInVertices];
		int[] triangles = new int[6 * (edgeSizeInQuads) * (edgeSizeInQuads)];
		for (int x = 0; x < edgeSizeInVertices; x++) {
			for (int z = 0; z < edgeSizeInVertices; z++) {
				int vertexIndex = x * edgeSizeInVertices + z;
				vertices [vertexIndex] = new Vector3 (x, 0, z);
				if (x < edgeSizeInQuads && z < edgeSizeInQuads) {
					CreateNewQuad (x, z, triangles, edgeSizeInVertices);
				}
			}
		}
		Mesh mesh = this.GetComponent<MeshFilter> ().mesh;
		mesh.Clear (); //forgetting this causes great frustration
		Destroy (this.gameObject.GetComponent<MeshCollider> ());
		mesh.vertices = vertices; //ALWAYS assign vertices before triangles
		mesh.triangles = triangles;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		this.gameObject.AddComponent<MeshCollider> ();
	}

	void ScaleTo (float newScale)
	{
		this.gameObject.transform.localScale = newScale * Vector3.one;
	}

	void CreateNewQuad (int x, int z, int[] triangles, int edgeSizeInVertices)
	{
		int edgeSizeInQuads = edgeSizeInVertices - 1; //avoid fencepost error
		int quadIndex = x * edgeSizeInQuads + z; 
		int quadIndexForArray = quadIndex * 6;
		int vertexLowerLeft = x * edgeSizeInVertices + z;

		//triangle 1 : lower left
		triangles [quadIndexForArray] = vertexLowerLeft;
		triangles [quadIndexForArray + 1] = vertexLowerLeft + 1;
		triangles [quadIndexForArray + 2] = vertexLowerLeft + edgeSizeInVertices;
		//triangle 2 : upper right
		triangles [quadIndexForArray + 3] = vertexLowerLeft + 1; 
		triangles [quadIndexForArray + 4] = vertexLowerLeft + edgeSizeInVertices + 1;
		triangles [quadIndexForArray + 5] = vertexLowerLeft + edgeSizeInVertices;
	}
}
