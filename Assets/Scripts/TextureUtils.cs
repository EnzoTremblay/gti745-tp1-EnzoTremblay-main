using UnityEngine;

public static class TextureUtils
{
     /// <summary>
    /// Inverse horizontalement une texture (effet miroir)
    /// </summary>
    /// <param name="original">La texture d'origine à inverser</param>
    /// <returns>Une nouvelle texture inversée horizontalement</returns>
    public static Texture2D FlipTextureHorizontally(Texture2D original)
    {
        if (original == null)
            return null;
            
        int width = original.width;
        int height = original.height;
                
        // Créer une nouvelle texture pour le résultat
        Texture2D flipped = new Texture2D(width, height);
        
        // Inverser les pixels horizontalement
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Obtenir la couleur du pixel à la position inversée
                Color pixel = original.GetPixel(width - 1 - x, y);
                flipped.SetPixel(x, y, pixel);
            }
        }
        
        // Appliquer les modifications
        flipped.Apply();
        
        return flipped;
    }

    /// <summary>
    /// Recadre et redimensionne une texture pour obtenir les dimensions souhaitées.
    /// Cette fonction conserve la partie centrale de l'image.
    /// </summary>
    /// <param name="inputTexture">La texture d'origine à traiter</param>
    /// <param name="targetWidth">Largeur cible souhaitée</param>
    /// <param name="targetHeight">Hauteur cible souhaitée</param>
    /// <returns>Une nouvelle texture aux dimensions souhaitées</returns>
    public static Texture2D CropAndResizeTexture(Texture2D inputTexture, int targetWidth, int targetHeight)
    {
        // Vérification des paramètres d'entrée
        if (inputTexture == null)
            return null;

        // Dimensions d'origine
        int originalWidth = inputTexture.width;
        int originalHeight = inputTexture.height;

        // Étape 1: Déterminer les dimensions de recadrage intermédiaire
        int cropWidth, cropHeight;
        int xOffset = 0, yOffset = 0;

        // Calculer le rapport d'aspect cible
        float targetAspect = (float)targetWidth / targetHeight;

        if (originalWidth / (float)originalHeight > targetAspect)
        {
            // L'image originale est plus large que l'aspect ratio cible
            // On garde la hauteur complète et on coupe les côtés
            cropHeight = originalHeight;
            cropWidth = Mathf.RoundToInt(originalHeight * targetAspect);
            xOffset = (originalWidth - cropWidth) / 2;
        }
        else
        {
            // L'image originale est plus haute que l'aspect ratio cible
            // On garde la largeur complète et on coupe en haut et en bas
            cropWidth = originalWidth;
            cropHeight = Mathf.RoundToInt(originalWidth / targetAspect);
            yOffset = (originalHeight - cropHeight) / 2;
        }

        // Étape 2: Recadrer la texture
        Color[] croppedPixels = new Color[cropWidth * cropHeight];
        for (int y = 0; y < cropHeight; y++)
        {
            for (int x = 0; x < cropWidth; x++)
            {
                croppedPixels[y * cropWidth + x] = inputTexture.GetPixel(x + xOffset, y + yOffset);
            }
        }

        // Étape 3: Créer une texture recadrée intermédiaire
        Texture2D croppedTexture = new Texture2D(cropWidth, cropHeight);
        croppedTexture.SetPixels(croppedPixels);
        croppedTexture.Apply();

        // Étape 4: Redimensionner à la taille cible
        Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight);
        
        // Option 1: Utiliser Graphics.Blit pour un redimensionnement de haute qualité
        // Cette méthode nécessite un RenderTexture, qui offre une meilleure qualité
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
        RenderTexture.active = rt;
        Graphics.Blit(croppedTexture, rt);
        resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        resizedTexture.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        
        // Nettoyage de la texture intermédiaire
        Object.Destroy(croppedTexture);
        
        return resizedTexture;
    }
}
