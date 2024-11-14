# Wake

Simple console Wake-on-LAN

## Usage

Edit `config.json` as below

```json
[
    {
      "NickName": "PC1",
      "MAC": "xx:xx:xx:xx:xx:xx"
    },
    {
      "NickName": "PC2",
      "MAC": "xx:xx:xx:xx:xx:xx"
    }
]
```
``` PowerShell
.\Wake.exe
Usage:  Wake list - list all available PCs
        Wake [nickname] - wake PC with specific nickname
```
