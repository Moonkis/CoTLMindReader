# CoTLMindReader
A save-file encryptor/decryptor for Cult of the Lamb

# Note
Please back up your files before using this, as it will overwrite the file. On Windows these files can be found under `AppData\LocalLow\Massive Monster\Cult Of The Lamb\saves`

The files are:
`meta_<number>.json`
`settings.json`
`slot_<number>.json`

`<number>` represents the save-slot, so first slot will be `0` and so on.

# Usage
Open up your terminal (Windows CMD) and type:

`CoTLMindReader D <PATH_TO_FILE>` to decrypt.
`CoTLMindReader E <PATH_TO_FILE>` to encrypt.

Where `<PATH_TO_FILE>` is replaced with the actual path to the file you want to encrypt/decrypt.
