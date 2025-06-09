public async Task<ImageSource> ReceiveImageAsync()
{
    TcpListener listener = new TcpListener(IPAddress.Loopback, 5001);
    listener.Start();

    using var client = await listener.AcceptTcpClientAsync();
    using var stream = client.GetStream();

    byte[] lengthBytes = new byte[4];
    await stream.ReadAsync(lengthBytes, 0, 4);
    int length = BitConverter.ToInt32(lengthBytes.Reverse().ToArray(), 0);

    byte[] data = new byte[length];
    int read = 0;
    while (read < length)
        read += await stream.ReadAsync(data, read, length - read);

    using var ms = new MemoryStream(data);
    var bitmap = new BitmapImage();
    bitmap.BeginInit();
    bitmap.CacheOption = BitmapCacheOption.OnLoad;
    bitmap.StreamSource = ms;
    bitmap.EndInit();
    bitmap.Freeze();
    return bitmap;
}
