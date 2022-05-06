# Alpha Release
Please refer to our [support policy](https://shimmersensing.com/support/wireless-sensor-networks-documentation/) on Alpha Releases

# How to use the file parser
1. Set the JAVA_HOME System Variable and set the JAVA_HOME System Path using the jre provided (can refer to this [guide](https://docs.oracle.com/goldengate/1212/gg-winux/GDRAD/java.htm#BGBFHBEA))
2. Open command prompt and navigate to the directory where the 'VerisenseFileParserPC.jar' is located
3. Use the same S3 folder structure to store the binary files (Participant ID > Sensor ID > BinaryFiles)
4. Run `java -jar VerisenseFileParserPC.jar <path> <optional arguments>` in the command prompt 
     - where `<path>` can be the any of the following
       - Path to the participant level folder
       - Path to the BinaryFiles level folder
       - Path to a specific Binary file
     - and `<optional arguments>` can be the any of the following
       - NO_CRC_CHECK (Skips CRC check when parsing payloads. Was used to debug bad bin files during early development)
       - DEBUG_PAYLOAD_PARSING (More detailed console feedback of payloads being parsed)
       - SAVE_DATA_BLOCK_METADATA (Payload metadata CSVs are saved by default to give overall information about payloads. This option turns on data block metadata CSVs to give access to lower level metadata on the data blocks that make up each payload)
       - STANDALONE (Used to indicate to the cloud version of the file parser that there is no trial database present and that any features that rely on it should be disabled)
5. For example, `java -jar VerisenseFileParserPC.jar C:/Users/username/Desktop/WW/19092501A2BB/BinaryFiles SAVE_DATA_BLOCK_METADATA`
6. The parsed files directory will be at the same level with the BinaryFiles level folder
7. The Algorithm folder contains the results for non-wear detection
8. For more details regarding the file parser output, can refer to 'ASM-DOC06-03_Verisense Data Dictionary.pdf'
