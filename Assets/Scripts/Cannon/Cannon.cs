using System.Collections.Generic;
using RDTools.AutoAttach;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Cannon
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] Transform _spawnPoint;
        [SerializeField] Bullet _bulletPrefab;

        [SerializeField] float _fireSpeed;
        [SerializeField] float _delay;

        public ObjectPool<Bullet> Pool { get; set; }

        [SerializeField, Attach()] ParticleSystemPool _particleSystemPool;

        List<Bullet> _instantiatedBullets = new();
        float _startDelay;

        void Awake()
        {
            Pool = new ObjectPool<Bullet>
                (SpawnBullet, OnTakeBulletFromPool, OnReturnBulletFromPool);
            
            _startDelay = _delay;
        }

        void OnDisable()
        {
            foreach (var instantiatedBullet in _instantiatedBullets)
            {
                instantiatedBullet.OnTriggeredBullet -= BulletRelease;
            }
        }

        void Update()
        {
            Shoot();
        }

        void Shoot()
        {
            _delay -= Time.deltaTime;

            if (_delay <= 0f)
            {
                _delay = _startDelay;
                GetBullet();
                _particleSystemPool.SetParticle(_spawnPoint);
            }
        }
        
        Bullet SpawnBullet()
        {
            var newBullet = Instantiate(_bulletPrefab, _spawnPoint.position, Quaternion.identity, transform);
            _instantiatedBullets.Add(newBullet);
            newBullet.OnTriggeredBullet += BulletRelease;
            return newBullet;
        }
        
        void GetBullet() => Pool.Get();

        void OnTakeBulletFromPool(Bullet bullet) => bullet.InitBullet(bullet, _fireSpeed);

        void OnReturnBulletFromPool(Bullet bullet) => bullet.DisableBullet(bullet, _spawnPoint);

        void BulletRelease(Bullet bullet) => Pool.Release(bullet);
    }
}