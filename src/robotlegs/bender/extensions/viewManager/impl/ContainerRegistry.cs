//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.viewManager.impl
{
	/// <summary>
	/// Container registry
	/// It holds a reference of all view containers on stage (in our case ContextView's)
	/// It should also hold a reference on which containers are parents of each other
	/// It also handles views to the correct container
	/// </summary>


	public class ContainerRegistry : IParentFinder
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<object> ContainerAdd;

		public event Action<object> ContainerRemove;

		public event Action<object> RootContainerAdd;

		public event Action<object> RootContainerRemove;

		public event Action<object> FallbackContainerAdd;

		public event Action<object> FallbackContainerRemove;

		public List<ContainerBinding> Bindings 
		{
			get 
			{ 
				return _bindings; 
			}
		}

		public List<ContainerBinding> RootBindings 
		{
			get 
			{ 
				return _rootBindings; 
			}
		}

		public ContainerBinding FallbackBinding
		{
			get
			{
				return _fallbackBinding;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private List<ContainerBinding> _bindings = new List<ContainerBinding>();

		private List<ContainerBinding> _rootBindings = new List<ContainerBinding>();

		private Dictionary<object, ContainerBinding> _bindingByContainer = new Dictionary<object, ContainerBinding>();

		private IParentFinder _parentFinder = new BlankParentFinder ();

		private ContainerBinding _fallbackBinding;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public ContainerBinding AddContainer(object container)
		{
			if (_bindingByContainer.ContainsKey(container))
				return _bindingByContainer[container];

			return _bindingByContainer[container] = CreateBinding(container);
		}

		public ContainerBinding RemoveContainer(object container)
		{
			ContainerBinding binding;
			_bindingByContainer.TryGetValue (container, out binding);
			if (binding != null)
			{
				RemoveBinding (binding);
			}
			return binding;
		}

		public ContainerBinding SetFallbackContainer(object container)
		{
			if (_fallbackBinding != null) 
			{
				RemoveBinding (_fallbackBinding);
			}

			if (container == null)
				return null;

			ContainerBinding binding;
			if (_bindingByContainer.TryGetValue (container, out binding))
				return _fallbackBinding = binding;

			return _fallbackBinding = _bindingByContainer[container] = CreateBinding(container, true);
		}

		public void RemoveFallbackContainer()
		{
			_fallbackBinding = null;
		}

		public ContainerBinding FindParentBinding(object container)
		{
			if (_parentFinder == null)
				return _fallbackBinding;

			object parent = _parentFinder.FindParent(container, _bindingByContainer);
			if (parent == null)
				return _fallbackBinding;
			ContainerBinding binding;
			if (!_bindingByContainer.TryGetValue (parent, out binding))
			{
				binding = _fallbackBinding;
			}
			return binding;
		}

		public ContainerBinding GetBinding(object container)
		{
			ContainerBinding binding;
			if (!_bindingByContainer.TryGetValue (container, out binding))
			{
				binding = _fallbackBinding;
			}
			return binding;
		}

		public void SetParentFinder(IParentFinder parentFinder)
		{
			_parentFinder = parentFinder;
		}

		public bool Contains (object parentContainer, object childContainer)
		{
			return _parentFinder.Contains (parentContainer, childContainer);
		}

		public object FindParent (object childView, Dictionary<object, ContainerBinding> containers)
		{
			return _parentFinder.FindParent(childView, containers);
		}

		public object FindParent (object childView, IEnumerable<ContainerBinding> containers)
		{
			return _parentFinder.FindParent(childView, containers);
		}


		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private ContainerBinding CreateBinding(object container, bool forceRoot = false)
		{
			ContainerBinding binding = new ContainerBinding(container);
			_bindings.Add(binding);

			binding.BINDING_EMPTY += OnBindingEmpty;

			if (!forceRoot) 
			{
				binding.Parent = FindParentBinding (container);
			}

			if (binding.Parent == null)
			{
				AddRootBinding(binding);
			}

			// Reparent any bindings which are contained within the new binding AND
			// A. Don't have a parent, OR
			// B. Have a parent that is not contained within the new binding
			foreach (ContainerBinding childBinding in _bindingByContainer.Values)
			{
				if (forceRoot || _parentFinder.Contains(container, childBinding.Container))
				{
					if (childBinding.Parent == null)
					{
						RemoveRootBinding(childBinding);
						childBinding.Parent = binding;
					}
					else if (!_parentFinder.Contains(container, childBinding.Parent.Container) && !forceRoot)
					{
						childBinding.Parent = binding;
					}
				}
			}

			if (ContainerAdd != null)
			{
				ContainerAdd (binding.Container);
			}
			if (FallbackContainerAdd != null) 
			{
				FallbackContainerAdd (binding.Container);
			}
			return binding;
		}

		private void RemoveBinding(ContainerBinding binding)
		{
			// Remove the binding itself
			_bindingByContainer.Remove (binding.Container);
			_bindings.Remove (binding);

			// Drop the empty binding listener
			binding.BINDING_EMPTY -= OnBindingEmpty;

			if (binding.Parent == null)
			{
				// This binding didn't have a parent, so it was a Root
				RemoveRootBinding (binding);
			}

			// Re-parent the bindings
			foreach (ContainerBinding childBinding in _bindingByContainer.Values)
			{
				if (childBinding.Parent == binding)
				{
					childBinding.Parent = binding.Parent;
					if (childBinding.Parent == null)
					{
						// This binding used to have a parent,
						// but no longer does, so it is now a Root
						AddRootBinding(childBinding);
					}
				}
			}

			if (ContainerRemove != null)
			{
				ContainerRemove (binding.Container);
			}

			if (binding == _fallbackBinding) 
			{
				_fallbackBinding = null;
				if (FallbackContainerRemove != null) 
				{
					FallbackContainerRemove (binding.Container);
				}
			}
		}

		private void AddRootBinding(ContainerBinding binding)
		{
			_rootBindings.Add(binding);
			if (RootContainerAdd != null)
			{
				RootContainerAdd (binding.Container);
			}
		}

		private void RemoveRootBinding(ContainerBinding binding)
		{
			_rootBindings.Remove (binding);
			if (RootContainerRemove != null)
			{
				RootContainerRemove (binding.Container);
			}
		}
		
		private void OnBindingEmpty(ContainerBinding binding)
		{
			RemoveBinding (binding);
		}
	}
}

