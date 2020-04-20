namespace StarForce
{
    public class ActiveBinding : BooleanBinding
    {
        protected override void ApplyNewValue(bool newValue)
        {
            gameObject.SetActive(newValue);
        }
    }
}