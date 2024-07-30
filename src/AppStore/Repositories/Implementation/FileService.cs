using AppStore.Repositories.Abstract;
namespace AppStore.Repositories.Implementation;

public class FileService(IWebHostEnvironment environment) : IFileService
{
    private readonly IWebHostEnvironment environment = environment;

    public Tuple<int, string> SaveImage(IFormFile imageFile)
    {
        try{
            var wwwPath = this.environment.WebRootPath;
            var path = Path.Combine(wwwPath, "Upload");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // tomar la extension del archivo
            var ext = Path.GetExtension(imageFile.FileName);
            // Extensiones permitidas para la carga de archivos
            var allowedExtensions = new String[] {".jpg", ".png", ".jpeg"};
            if(!allowedExtensions.Contains(ext))
            {
                var message = $"Solo estan permitidas las extensiones {allowedExtensions}";
                return new Tuple<int, string>(0, message);
            }

            // crear un nombre unico a cada imagen
            var uniqueString = Guid.NewGuid().ToString();
            var newFileName = uniqueString + ext;
            var fileWithPath = Path.Combine(path, newFileName);

            var stream = new FileStream(fileWithPath, FileMode.Create);
            imageFile.CopyTo(stream);
            stream.Close();

            return new Tuple<int, string>(1, newFileName);

        }catch(Exception)
        {
            return new Tuple<int, string>(0, "Error al guardar imagen");
            
        }
    }

    public bool DeleteImage(string imageFileName)
    {
        try{
            var wwwPath = environment.WebRootPath;
            var path = Path.Combine(wwwPath, "Upload//", imageFileName);
            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }

            return false;
        }catch(Exception)
        {
            return false;
        }
    }

}