﻿<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Drawing;
using System.Web;
using ImageUploadAndCrop;

public class ImageHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        byte[] image = GetImage(context.Request.QueryString["id"], context);
        context.Response.Clear();
        context.Response.ContentType = "image/pjpeg";
        context.Response.BinaryWrite(image);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

    private byte[] GetImage(string id, HttpContext context)
    {
        // Create a new, empty image to return as default
        byte[] image = new byte[0];

        string imageKey = string.Format("{0}{1}", "ImageKey_", id);
        if (context.Cache[imageKey] != null)
        {
            object imgObj = context.Cache[imageKey];
            if (imgObj.GetType() == typeof(byte[]))
                image = (byte[])imgObj;
            else if (imgObj.GetType() == typeof(Bitmap))
            {
                using (Image tempImage = (Image)imgObj)
                {
                    image = web.Areas.Admin.ImageHelperNew.ImageToByteArray(tempImage, string.Empty, null);
                }
            }
        }
        
        return image;
    }
}