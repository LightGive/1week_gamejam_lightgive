using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer)),ExecuteInEditMode]
public class FlatFX:MonoBehaviour{
	public List<FlatFXParticle> particles=new List<FlatFXParticle>(10);
	private Mesh mesh;
	private List<Vector3> vertices;
	private List<Color> colors;
	private List<Vector3> uvs;
	private int[] triangles;
	//Keeps track of which effect is selected
	public int selectedEffect=0;
	//Setting for all effects
	public List<FlatFXSettings> settings;
	//Components
	private MeshRenderer mr;
	private MeshFilter mf;
	//Material to use
	private Material material;
	//To count and display total triangles
	private int totalTriangles=0;
	//For sorting layers
	public int sortingLayer=0;
	private int _sortingLayer;
	public int orderInLayer=0;
	private int _orderInLayer;
	void Awake(){
		mr=GetComponent<MeshRenderer>();
		mf=GetComponent<MeshFilter>();
		mesh=new Mesh();
		vertices=new List<Vector3>(500);
		colors=new List<Color>(500);
		uvs=new List<Vector3>(500);
		mf.sharedMesh=mesh;
	}

	public void SetMaterial(Material material){
		this.material=material;
		mr.sharedMaterial=this.material;
	}

	public void AddEffect(Vector2 position,int effectNumber){
		Vector2 randomPlace=Random.insideUnitCircle*settings[effectNumber].randomizePosition;
		float seed=Random.value;
		if(settings[effectNumber].type==FlatFXType.SunRays){
			settings[effectNumber].start.rotation=Random.Range(0f,360f);
			settings[effectNumber].end.rotation=settings[effectNumber].start.rotation-60f*settings[effectNumber].lifetime;
		}else{
			settings[effectNumber].start.rotation=Random.Range(0f,360f);
			settings[effectNumber].end.rotation=settings[effectNumber].start.rotation;
		}
		particles.Add(new FlatFXParticle(
			settings[effectNumber].type,
			settings[effectNumber].lifetime,
			settings[effectNumber].sectorCount,
			settings[effectNumber].easing,
			new FlatFXState(
				transform.InverseTransformPoint(position+randomPlace),
				settings[effectNumber].start.size,
				settings[effectNumber].start.thickness,
				settings[effectNumber].start.rotation,
				settings[effectNumber].start.innerColor,
				settings[effectNumber].start.outerColor,
				seed
			),
			new FlatFXState(
				transform.InverseTransformPoint(position+randomPlace),
				settings[effectNumber].end.size,
				settings[effectNumber].end.thickness,
				settings[effectNumber].end.rotation,
				settings[effectNumber].end.innerColor,
				settings[effectNumber].end.outerColor,
				seed
			)
		));
	}

	public int particleCount{
		get{return particles.Count;}
	}

	public int triangleCount{
		get{return totalTriangles/3;}
	}

	void Update(){
		if(sortingLayer!=_sortingLayer || orderInLayer!=_orderInLayer){
			mr.sortingLayerID=sortingLayer;
			mr.sortingOrder=orderInLayer;
			_sortingLayer=sortingLayer;
			_orderInLayer=orderInLayer;
		}
		MakeMesh();
	}

	//Combine all the data particles generate into a single mesh
	private void MakeMesh(){
		if(particles.Count==0 && mesh.vertices.Length==0) return;
		//Remove dead particles
		for(int i=0;i<particles.Count;i++){
			if(particles[i].isDead){
				particles.RemoveAt(i);
			}
		}
		//Generate the shapes and count total number of triangles in them
		totalTriangles=0;
		if(particles.Count>0){
			for(int i=0;i<particles.Count;i++){
				particles[i].Generate();
				if(particles[i].triangles!=null){
					totalTriangles+=particles[i].triangles.Length;
				}
			}
		}
		//Prepare all lists
		if(mesh==null) mesh=new Mesh();
		mesh.Clear();
		vertices.Clear();
		colors.Clear();
		uvs.Clear();
		triangles=new int[totalTriangles];
		//Get all mesh data: vertices, colors, triangles
		int prevVertices=0;
		int countTriangles=0;
		if(particles.Count>0){
			for(int i=0;i<particles.Count;i++){
				//for(int i=particles.Count-1;i>=0;i--){
				for(int j=0;j<particles[i].vertices.Count;j++){
					vertices.Add(particles[i].vertices[j]);
					colors.Add(particles[i].colors[j]);
					uvs.Add(Vector3.zero); //Setting UVs to zero since we don't do textures
				}
				if(particles[i].triangles!=null){
					for(int j=0;j<particles[i].triangles.Length;j++){
						triangles[countTriangles]=particles[i].triangles[j]+prevVertices;
						countTriangles++;
					}
				}
				prevVertices+=particles[i].vertices.Count;
			}
		}
		//Set mesh data
		mesh.SetVertices(vertices);
		mesh.SetColors(colors);
		mesh.SetUVs(0,uvs);
		//mesh.RecalculateNormals();
		mesh.bounds=new Bounds();
		mesh.SetTriangles(triangles,0);
	}

}
