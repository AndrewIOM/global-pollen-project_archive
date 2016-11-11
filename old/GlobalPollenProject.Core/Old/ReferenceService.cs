using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.Core
{
    public class ReferenceService : IReferenceService
    {
        private IUnitOfWork _uow;
        public ReferenceService(IUnitOfWork uow, ITaxonomyService taxonomyService)
        {
            _uow = uow;
        }

        public ReferenceCollection AddCollection(ReferenceCollection collection)
        {
            _uow.RefCollectionRepository.Add(collection);
            _uow.SaveChanges();
            return _uow.RefCollectionRepository.GetAll().OrderBy(i => i.Id).Last();
        }

        public ReferenceGrain AddGrain(ReferenceGrain grain)
        {
            // var standardImages = await _fileService.UploadBase64Image(result.Images);
            // var taxon = _taxonomyService.CreateOrUpdateTaxonomy(result.Family, result.Genus, result.Species);
            // var toSave = new ReferenceGrain()
            // {
            //     Collection = collection,
            //     Taxon = taxon,
            //     SubmittedBy = _userService.GetById(UserManager.GetUserId(User)),
            //     TimeAdded = DateTime.Now,
            //     MaxSizeNanoMetres = result.MaxGrainSize.Value,
            //     Images = new List<GrainImage>()
            // };

            // foreach (var file in standardImages)
            // {
            //     toSave.Images.Add(new GrainImage()
            //     {
            //         FileName = file.Url,
            //         FileNameThumbnail = file.ThumbnailUrl,
            //         IsFocusImage = false
            //     });
            // }

            // foreach (var image in result.FocusImages)
            // {
            //     var low = await _fileService.UploadBase64Image(image.FocusLowUrl);
            //     var medLow = await _fileService.UploadBase64Image(image.FocusMedLowUrl);
            //     var med = await _fileService.UploadBase64Image(image.FocusMedUrl);
            //     var medHigh = await _fileService.UploadBase64Image(image.FocusMedHighUrl);
            //     var high = await _fileService.UploadBase64Image(image.FocusHighUrl);
            //     toSave.Images.Add(new GrainImage()
            //     {
            //         FileName = med.Url,
            //         FileNameThumbnail = med.ThumbnailUrl,
            //         IsFocusImage = true,
            //         FocusLowUrl = low.Url,
            //         FocusMedLowUrl = medLow.Url,
            //         FocusMedUrl = med.Url,
            //         FocusMedHighUrl = medHigh.Url,
            //         FocusHighUrl = high.Url
            //     });
            // }

            // var saved = _refService.AddGrain(toSave);


            _uow.RefGrainRepository.Add(grain);
            _uow.SaveChanges();
            return _uow.RefGrainRepository.GetAll().OrderBy(i => i.ReferenceGrainId).Last();
        }

        public void DeleteCollection(int collectionId)
        {
            var existing = _uow.RefCollectionRepository.GetById(collectionId);
            if (existing != null)
            {
                foreach (var grain in existing.Grains)
                {
                    _uow.RefGrainRepository.Delete(grain);
                }
                _uow.RefCollectionRepository.Delete(existing);
            }
            _uow.SaveChanges();
        }

        public void DeleteGrain(int grainId)
        {
            var existing = _uow.RefGrainRepository.GetById(grainId);
            if (existing != null)
            {
                _uow.RefGrainRepository.Delete(existing);
            }
            _uow.SaveChanges();
        }

        public ReferenceCollection GetCollectionById(int id)
        {
            return _uow.RefCollectionRepository.GetById(id);
        }

        public List<ReferenceCollection> GetCollectionsByUser(string userId)
        {
            return _uow.RefCollectionRepository.Find(m => m.User.Id == userId).ToList();
        }

        public ReferenceGrain GetGrainById(int id)
        {
            return _uow.RefGrainRepository.GetById(id);
        }

        public ReferenceCollection UpdateCollection(ReferenceCollection collection)
        {
            _uow.RefCollectionRepository.Update(collection);
            _uow.SaveChanges();
            return _uow.RefCollectionRepository.GetById(collection.Id);
        }

        public ReferenceGrain UpdateGrain(ReferenceGrain grain)
        {
            _uow.RefGrainRepository.Update(grain);
            _uow.SaveChanges();
            return _uow.RefGrainRepository.GetById(grain.ReferenceGrainId);
        }

        private string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
        }

        private string FirstCharToLower(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToLower() + input.Substring(1).ToLower();
        }

        public List<ReferenceCollection> ListCollections()
        {
            return _uow.RefCollectionRepository.GetAll().ToList();
        }
    }
}
