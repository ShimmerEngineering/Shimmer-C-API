# How to use the file parser
1. Set the JAVA_HOME System Variable and set the JAVA_HOME System Path using the jre provided (can refer to this [guide](https://docs.oracle.com/goldengate/1212/gg-winux/GDRAD/java.htm#BGBFHBEA))
2. Open command prompt and navigate to the directory where the 'VerisenseFileParserPC.jar' is located
3. Run `java -jar VerisenseFileParserPC.jar <path>` in the command prompt where `<path>` can be the any of the
     - Path to the participant level folder
     - Path to the BinaryFiles level folder
     - Path to a specific Binary file
4. For example, `java -jar VerisenseFileParserPC.jar C:/Users/username/Desktop/WW/19092501A2BB/BinaryFiles`
5. The parsed files directory will be at the same level with the binaryFiles level folder

