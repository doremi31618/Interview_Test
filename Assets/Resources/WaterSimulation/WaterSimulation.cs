using UnityEngine;

public class WaterSimulation : MonoBehaviour
{
    [SerializeField]
    public CustomRenderTexture texture;
    public Texture2D myTexture2D;
    public LayerMask detectLayer;

    [SerializeField]
    int iterationPerFrame = 5;

    void Start()
    {
        texture.Initialize();
        myTexture2D = new Texture2D(texture.width, texture.height);
    }

    void Update()
    {
        texture.ClearUpdateZones();
        UpdateZones();
        texture.Update(iterationPerFrame);
        myTexture2D = TextureToTexture2D(texture);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            texture.Initialize();
        }
    }

    void UpdateZones()
    {
        bool leftClick = Input.GetMouseButton(0);
        bool rightClick = Input.GetMouseButton(1);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (!leftClick && !rightClick) 
        {
            if (Physics.Raycast(ray, out hit,Mathf.Infinity,detectLayer)) {
            
                Debug.DrawLine(Camera.main.transform.position,hit.point,Color.blue);
                //Debug.Log(myTexture2D.GetPixel((int)hit.point.x,(int)hit.point.z));
                //Debug.Log(new Vector2((int)hit.point.x,(int)hit.point.z));
            }   
        }
        else
        {
            if (Physics.Raycast(ray, out hit,Mathf.Infinity,detectLayer)) {
                var defaultZone = new CustomRenderTextureUpdateZone();
                defaultZone.needSwap = true;
                defaultZone.passIndex = 0;
                defaultZone.rotation = 0f;
                defaultZone.updateZoneCenter = new Vector2(0.5f, 0.5f);
                defaultZone.updateZoneSize = new Vector2(1f, 1f);

                var clickZone = new CustomRenderTextureUpdateZone();
                clickZone.needSwap = true;
                clickZone.passIndex = leftClick ? 1 : 2;
                clickZone.rotation = 0f;
                clickZone.updateZoneCenter = new Vector2(hit.textureCoord.x, 1f - hit.textureCoord.y);
                clickZone.updateZoneSize = new Vector2(0.01f, 0.01f);

                texture.SetUpdateZones(new CustomRenderTextureUpdateZone[] { defaultZone, clickZone });
                
               
                Debug.DrawLine(Camera.main.transform.position,hit.point,Color.red);
                // Vector2Int pointMatrix = new Vector2Int(
                //     (int)(hit.point.x + texture.width/2),
                //     (int)(hit.point.z + texture.height/2));
                //Debug.Log(myTexture2D.GetPixel(0,1));
                //Debug.Log(pointMatrix);
                
            }   
        }

        
        
        
    }
    private Texture2D TextureToTexture2D(Texture texture) {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
}
}
public static class TextureExtentions
     {
         public static Texture2D ToTexture2D(this Texture texture)
         {
             return Texture2D.CreateExternalTexture(
                 texture.width,
                 texture.height,
                 TextureFormat.RGB24,
                 false, false,
                 texture.GetNativeTexturePtr());
         }
     }