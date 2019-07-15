
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;


public class  DrawLineAndVertex : MonoBehaviour
{
    Material mat;
    void Start()
    {
        //RenderPipeline.beginCameraRendering = OnPostProcess(Camera.main);
    }

    void OnPostProcess(Camera camera)
    {
        Vector3 _startPos = new Vector3();
        Vector3 _endPos = new Vector3();
        Color _color = new Color();
        OnLineRender(mat, _startPos, _endPos, _color);
    }

    public void OnLineRender(Material _mat, Vector3 _startPos, Vector3 _endPos, Color _color)
    {
        //RenderPipeline.beginCameraRendering = OnPostProcess(Camera.main);
        GL.PushMatrix();
        GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
        GL.modelview = Camera.main.worldToCameraMatrix;

        mat.SetPass(0);

        GL.Begin(GL.LINES);
        GL.Color(_color);
        GL.Vertex3(_startPos.x, _startPos.y, _startPos.z);
        GL.Vertex3(_endPos.x, _endPos.y, _endPos.z);
        GL.End();

        GL.PopMatrix();
    }
}
