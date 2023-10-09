# Wake

Simple console Wake-on-LAN

## Usage

Change `config.json` as below

```json
{
  "BroadcastIP": "your LAN broadcast IP",
  "PCList": [
    {
      "NickName": "PC1",
      "MAC": "xx:xx:xx:xx:xx:xx"
    },
    {
      "NickName": "PC2",
      "MAC": "xx:xx:xx:xx:xx:xx"
    }
  ]
}
```
``` PowerShell
.\Wake.exe
# ./Wake
Usage:  Wake list - list all available PCs
        Wake [nickname] - wake PC with specific nickname
```
