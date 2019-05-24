﻿using System.Collections.ObjectModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.Catches
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MediaPage : ContentPage
	{
		ObservableCollection<MediaFile> files = new ObservableCollection<MediaFile>();
		public MediaPage()
		{
			InitializeComponent();

			files.CollectionChanged += Files_CollectionChanged;


			takePhoto.Clicked += async (sender, args) =>
			{
				files.Clear();
				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				{
					await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
					return;
				}

				var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
				{
					PhotoSize = PhotoSize.Medium,
					Directory = "Sample",
					Name = "test.jpg"
				});

				if (file == null)
					return;

				await DisplayAlert("File Location", file.Path, "OK");

				files.Add(file);
			};

			pickPhoto.Clicked += async (sender, args) =>
			{
				files.Clear();
				if (!CrossMedia.Current.IsPickPhotoSupported)
				{
					await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
					return;
				}
				var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
				{
					PhotoSize = PhotoSize.Medium
				});


				if (file == null)
					return;

				files.Add(file);
			};

			/*pickPhotos.Clicked += async (sender, args) =>
			{
				files.Clear();
				if (!CrossMedia.Current.IsPickPhotoSupported)
				{
					await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
					return;
				}
				var picked = await CrossMedia.Current.PickPhotosAsync();


				if (picked == null)
					return;
				foreach (var file in picked)
					files.Add(file);
				
			};*/

			takeVideo.Clicked += async (sender, args) =>
			{
				files.Clear();
				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
				{
					await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
					return;
				}

				var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
				{
					Name = "video.mp4",
					Directory = "DefaultVideos",
				});

				if (file == null)
					return;

				await DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

				file.Dispose();
			};

			pickVideo.Clicked += async (sender, args) =>
			{
				files.Clear();
				if (!CrossMedia.Current.IsPickVideoSupported)
				{
					await DisplayAlert("Videos Not Supported", ":( Permission not granted to videos.", "OK");
					return;
				}
				var file = await CrossMedia.Current.PickVideoAsync();

				if (file == null)
					return;

				await DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
				file.Dispose();
			};
		}

		private void Files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if(files.Count == 0)
			{
				ImageList.Children.Clear();
				return;
			}
			if (e.NewItems.Count == 0)
				return;

			var file = e.NewItems[0] as MediaFile;
			var image = new Image { WidthRequest = 300, HeightRequest = 300, Aspect = Aspect.AspectFit };
			image.Source = ImageSource.FromStream(() =>
			{
				var stream = file.GetStream();
				return stream;
			});
			ImageList.Children.Add(image);
		}
	}
}