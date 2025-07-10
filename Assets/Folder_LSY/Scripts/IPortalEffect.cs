using System.Collections;

public interface IPortalEffect
{
    IEnumerator PlayBeforeTeleport();
    IEnumerator PlayAfterTeleport();
}
