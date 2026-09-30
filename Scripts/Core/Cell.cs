/// ---------------------------------------------
/// Contact: Henry Braun
/// Brief: Defines an Cell
/// Thanks to VHLab for original implementation
/// Date: November 2017 
/// ---------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System;

namespace Biocrowds.Core
{
    public class Cell : MonoBehaviour
    {
        public int X;
        public int Y;
        public int Z;

        private List<Auxin> _auxins = new List<Auxin>();
        public int count;
        [SerializeField]
        private MeshRenderer _meshRenderer;

        public Bounds? _bounds = null;
        public Bounds Bounds => _bounds ??= GetBounds();

        public List<Auxin> Auxins
        {
            get { return _auxins; }
            set { _auxins = value; }
        }

        public void ShowMesh(bool _show)
        {
            _meshRenderer.enabled = _show;
        }

        private void Update()
        {
            count = _auxins.Count;
        }

        public Bounds GetBounds()
        {
            // faz aquela troca de Y e Z do objeto ser virado e rotacionado
            return new Bounds(transform.position, new Vector3(transform.lossyScale.x, transform.lossyScale.z, transform.lossyScale.y));
        }
    }
}