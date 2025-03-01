# Tool Usage

**Important:** The *Select Game:* options determines how this tool's options functions. make sure to select the right game from this option before using any of the tool's functions.

## Filelist tools

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

---will be added shortly---
