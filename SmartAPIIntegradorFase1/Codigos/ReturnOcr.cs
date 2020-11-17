public class ReturnOcr
{
    public int StatusCode { get; set; }

    public string StatusMessage { get; set; }

    public object Result { get; set; }

    public string Placa { get; set; }

    public byte[] Image { get; set; }

    public int IdCapturaImagemOcr { get; set; }
}
