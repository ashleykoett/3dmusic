using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationarySineFollow : SineFollow
{
    private bool _inRange = false;
    public override void Update()
    {
        if(!base._followActive) return;

        _targetPosition = player.transform.position;

        if(Vector3.Distance(_targetPosition, transform.position) > base.followDistance && _inRange)
        {
            base.characterSoundController.FadeOutSound();
            _inRange = false;
            return;
        }
        if(Vector3.Distance(_targetPosition, transform.position) < base.followDistance && !_inRange)
        {
            _inRange = true;
            base.characterSoundController.PlayNextSound();
        }

        // if (baseBehavior) baseBehavior.MoveCharacter(Vector3.zero);
        if (baseBehavior != null) baseBehavior.MoveCharacter(Vector3.zero);

        _t += Time.deltaTime;
        Vector3 pos = transform.position;
        pos.x += movementSpeed * Time.deltaTime;
        pos.y = _initialPosition.y + Mathf.Sin(_t / period) * amplitude;

        transform.position = pos;
    }
}
