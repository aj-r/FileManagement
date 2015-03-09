# FileManagement

Simple but flexible file management system. Includes recent file history.

## Usage

1. Choose a serializer.

  ```
  ISerializer serializer = new BinarySerializer();
  ```

1. Implement IFile in any objects you want to save.

  ```
  public class SomeObject : IFile
  {
      public string FilePath { get; set; }
  }
  ```

1. Save a file.

  ```
  IFileManager fileManager = new FileManager(serializer);
  SomeObject obj = new SomeObject { FilePath = "file.txt" };
  fileManager.Save(obj);
  ```

1. Load a file.

  ```
  IFileManager fileManager = new FileManager(serializer);
  SomeObject obj = fileManager.Load<SomeObject>("file.txt");
  ```

1. Access recent file history.

  ```
  IFileManager fileManager = new FileManager(serializer);
  foreach (string filePath in fileManager.GetRecentFiles())
  {
      // Do something
  }
  ```

  Any time you save or load a file, it is added to the recent file history (unless you explicitly tell it not to).
  
  Recent file history is saved in a text file. By default it is saved as 'recent.txt' in the current directory, but you can override this by setting FileManager.RecentFilesStoragePath 