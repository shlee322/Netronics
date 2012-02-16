namespace Netronics.Channel
{
    /// <summary>
    /// <see cref="Netronics"/>�� �ٽ� �������̽��̸� �ٸ� Ŭ���̾�Ʈ�� ����� �� �ִ� ����̴�.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// <see cref="IChannel"/>�� ����� ��� ȣ��Ǵ� �޼���
        /// </summary>
        void Connect();

        /// <summary>
        /// <see cref="IChannel"/>�� ���� ������ ��� ȣ��Ǵ� �޼���
        /// </summary>
        void Disconnect();

        /// <summary>
        /// <see cref="IChannel"/>���� �޽����� �����ϴ� �޼���
        /// </summary>
        /// <param name="message"><see cref="IChannel"/>���� ������ �޽���</param>
        void SendMessage(dynamic message);
    }
}