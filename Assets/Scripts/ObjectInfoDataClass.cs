[System.Serializable]
public class ObjectInfoDataClass
{
    public string ID;

    public enum ObjectTypeEnum {Test, Candy, Book, Bottle };
    public ObjectTypeEnum ObjectType;

    public enum ObjectColorEnum { Test, Red, Blue, Black, Yellow };
    public ObjectColorEnum ObjectColor;

    public PickUpObject Object;
}
