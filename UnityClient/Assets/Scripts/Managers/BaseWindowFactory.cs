using System;
using UnityClient.ViewModels;
using UnityClient.ViewModels.Windows;
using Zenject;

namespace UnityClient.Managers {
	public class BaseWindowFactory : PlaceholderFactory<Type, BaseWindow> {}
}