# Tool Usage

**Important:** The *Select Game:* options determines how this tool's options functions. make sure to select the right game from this option before using any of the functions.

## Filelist tools
This section contains tools that allows you to unpack the filelist data either in JSON format or in TXT format. 

### JSON options
- ***Unpack as JSON*** option will unpack the filelist file as a json file. the necessary data to rebuild the data back to the filelist file will all be present in this JSON file.
- The ***Repack JSON*** option will repack the json data, to the trilogy's filelist format.

#### JSON structures
The structure of the unpacked JSON file will differ based on the selected game.

**XIII-1**
```json
{
  "fileCount": 4,
  "chunkCount": 1,
  "data": {
    "Chunk_0": [
      {
        "fileCode": 553656330,
        "filePath": "0:100:76:mot/pc/sk_c001_lt/lsdpack.bin"
      },
      {
        "fileCode": 553656897,
        "filePath": "1:48100:2e060:mot/pc/sk_c001_lt/s1.white.win32.bin"
      },
      {
        "fileCode": 553665089,
        "filePath": "5e:80:e:mot/pc/sk_c002_sn/s1.white.win32.bin"
      },
      {
        "fileCode": 553665121,
        "filePath": "5f:1000:3cf:mot/pc/sk_c002_sn/t1.white.win32.bin"
      }
    ]
  }
}
```

**XIII-2** and **XIII-LR**
```json
{
  "encrypted": true,
  "seedA": 14468517168260231925,
  "seedB": 6983995975973249367,
  "encryptionTag(DO_NOT_CHANGE)": 501232760,
  "fileCount": 5,
  "chunkCount": 1,
  "data": {
    "Chunk_0": [
      {
        "fileCode": 16785408,
        "fileTypeID": 16,
        "filePath": "0:4e9a68:27052f:chr/pc/c001/bin/c001.win32.imgb"
      },
      {
        "fileCode": 16785664,
        "fileTypeID": 16,
        "filePath": "4e1:1524a3:96de8:chr/pc/c001/bin/c001.win32.trb"
      },
      {
        "fileCode": 16786432,
        "fileTypeID": 16,
        "filePath": "60f:3d7f5:c0ed:chr/pc/c001/bin/c001_def.win32.mpk"
      },
      {
        "fileCode": 16786433,
        "fileTypeID": 16,
        "filePath": "628:3f515:cfc8:chr/pc/c001/bin/c001_rain.win32.mpk"
      },
      {
        "fileCode": 16786434,
        "fileTypeID": 16,
        "filePath": "642:3fff5:d3f8:chr/pc/c001/bin/c001_snow.win32.mpk"
      }      
    ]
  }
}
```

### TXT options
- The ***Unpack as TXTs*** option will unpack the filelist file into multiple text files depending on the number of chunks present in the filelist. an #info.txt file containing core information about the filelist, will be generated next to the Chunk_## text files. all of these files will be unpacked inside a folder next to the filelist file.
- The ***Repack TXTs*** option will repack the files inside the folder, to the trilogy's filelist format.

#### TXT structures
The structure of the data in the Chunk_## text files and the #info.txt file, will differ based on the selected game.

**XIII-1**
- #info.txt
```
fileCount: 4
chunkCount: 1
```

- Chunk_##.txt
```
553656897|0:48100:2e060:mot/pc/sk_c001_lt/s1.white.win32.bin
553665089|5d:80:e:mot/pc/sk_c002_sn/s1.white.win32.bin
553665121|5e:1000:3cf:mot/pc/sk_c002_sn/t1.white.win32.bin
553673281|5f:80:e:mot/pc/sk_c003_sz/s1.white.win32.bin
```

**XIII-2** and **XIII-LR**
- #info.txt
```
encrypted: true
seedA: 14468517168260231925
seedB: 6983995975973249367
encryptionTag(DO_NOT_CHANGE): 501232760
fileCount: 5
chunkCount: 1
```

- Chunk_##.txt
```
16785408|16|0:4e9a68:27052f:chr/pc/c001/bin/c001.win32.imgb
16785664|16|4e1:1524a3:96de8:chr/pc/c001/bin/c001.win32.trb
16786432|16|60f:3d7f5:c0ed:chr/pc/c001/bin/c001_def.win32.mpk
16786433|16|628:3f515:cfc8:chr/pc/c001/bin/c001_rain.win32.mpk
16786434|16|642:3fff5:d3f8:chr/pc/c001/bin/c001_snow.win32.mpk
```

## Path Generator tools
These options help in generating the necessary path data for a supported path type (see [supported paths](https://github.com/Surihix/WhiteFilelistManager/blob/master/Docs/SupportedPaths.md)), in JSON or TXT format. the generated data can then have to be copied inside the unpacked filelist-JSON file or in one the Chunk_## text files, if you had used the ***Unpack as TXTs*** option. 

### Single Mode
- The ***Generate JSON Output*** will create the necessary path data that can be easily be copied into the JSON file, if you had used the ***Unpack as JSON*** option. 
- The ***Generate TXT Output*** will create the necessary path data that can be easily be copied into the Chunk_## text file, if you had used the ***Unpack as TXTs*** option. 

### Batch Mode
- The same type of options would also be present in the **Batch Mode** tab and you can use that to generate multiple paths data by providing the folder containing your new files with the correct virtual directory.
- When generating id based paths with the **Batch Mode** tab's options, you have to create a txt file called '#id-based_paths' and have the id based paths be written there along with the id number. the `|` character should be used to separate the path and your id.
<br>
For example, if you are generating new sound file paths, then your #id-based_paths text file should be written like this:
```
sound/pack/2028/snls160foley02.win32.scd|5
sound/pack/2069/snls140foley02.win32.scd|8
sound/pack/2069/snls140add04.win32.scd|26
```

#### Tips
- Ensure that you have set the correct game from the **Select Game** section of this tool before generating the path data.

- Before you add in your newly generated path data, decide whether you want to repack the new file into the white archive or if you want to use the layeredFS plugin to sideload the new file at runtime. if you are going with the former option, then make sure to unpack the white_img archive first and copy the new file with the correct virtual directory.

- Its highly recommended to first try and get used to the **Single Mode** tab's options. once you are familiar enough with this process, you can then use the **Batch Mode** tab's options.

- For paths that require an id, you can use a number that is somewhat closer to the max limit specified in the Number Input window, which you will get as soon as you try generating path data for such types of paths.

- For paths that are zone based, if your new path is adding a new zone into the game, then you have to specify your new zone's id in the input window when prompted.

- The path data has to be pasted in the JSON or Chunk_## text file according to the filecode sequence. (this will be the first number after the `|` character, if you had used the ***Generate TXT Output*** option)
<br>For example let's say your generated path data has a filecode value of `152900` and there is a filecode and path data in the JSON or Chunk_## text file, that has a filecode value of `152800`. your newly generated path data would have to be after the `152800` path data.

- If you are repacking the new file back into the white archive and you want your file to be compressed when repacking, then you can set the third `0` from the virtual path data, to `1`. for example, if your generated path data's virtual path is `0:0:0:mot/pc/sk_c001_lt/s1.white.win32.bin`, then it should be `0:0:1:mot/pc/sk_c001_lt/s1.white.win32.bin`. make sure you do not do this for `.scd` type files as they should always be stored uncompressed inside the white archive.

- After you have added your necessary path data, use the appropriate Repack options from the **Filelist Tools** section, to rebuild the data back to filelist format with the new file path(s). if you are trying to repack the file back into the white archive, you can use [WhiteBinTool's](https://github.com/Surihix/WhiteBinTools) `-r` switch's function, to repack new files into the archive using the newly rebuilt filelist.

- For any questions regarding this tool usage, general modding questions regarding the trilogy or for technical support, please reach out via the **Fabula Nova Crystalis modding community** discord server.
