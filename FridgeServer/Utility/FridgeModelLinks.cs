using Contracts;
using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FridgeServer.Utility
{
    public class FridgeModelLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<FridgeModelDto> _dataShaper;

        public FridgeModelLinks(LinkGenerator linkGenerator,
            IDataShaper<FridgeModelDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        private List<Entity> ShapeData(IEnumerable<FridgeModelDto> fridgeModelsDto,
            string fields) =>
            _dataShaper.ShapeData(fridgeModelsDto, fields).Select(i => i.Entity).ToList();

        public LinkResponse TryGenerateLinks(IEnumerable<FridgeModelDto> fridgeModelsDto,
            string fields, Guid fridgeId, HttpContext httpContext)
        {
            var shapedFridgeModels = ShapeData(fridgeModelsDto, fields);

            if (ShouldGenerateLinks(httpContext))
            {
                return ReturnLinkedFridgeModels(fridgeModelsDto, fields, fridgeId, httpContext,
                    shapedFridgeModels);
            }

            return ReturnShapedFridgeModels(shapedFridgeModels);
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",
                StringComparison.InvariantCultureIgnoreCase);
        }

        private LinkResponse ReturnShapedFridgeModels(List<Entity> shapedFridgeModels) =>
            new LinkResponse { ShapedEntities = shapedFridgeModels };

        private LinkResponse ReturnLinkedFridgeModels(IEnumerable<FridgeModelDto> fridgeModelsDto,
            string fields, Guid fridgeId, HttpContext httpContext, List<Entity> shapedFridgeModels)
        {
            var fridgeModelDtoList = fridgeModelsDto.ToList();

            for (var index = 0; index < fridgeModelDtoList.Count(); index++)
            {
                var fridgeModelLinks = CreateLinksForFridgeModels(httpContext, fridgeId,
                    fridgeModelDtoList[index].Id, fields);
                shapedFridgeModels[index].Add("Links", fridgeModelLinks);
            }

            var fridgeModelCollection = new LinkCollectionWrapper<Entity>(shapedFridgeModels);
            var linkedFridgeModels = CreateLinksForFridgeModels(httpContext, fridgeModelCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedFridgeModels };
        }

        private List<Link> CreateLinksForFridgeModels(HttpContext httpContext, Guid fridgeId,
            Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext,
                "GetFridgeModelForFridge",
                values: new { fridgeId, id, fields }),
                "self",
                "GET"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "DeleteFridgeModelForFridge",
                values: new { fridgeId, id }),
                "delete_fridge_model",
                "DELETE"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "UpdateFridgeModelForFridge",
                values: new { fridgeId, id }),
                "update_fridge_model",
                "PUT"),

                new Link(_linkGenerator.GetUriByAction(httpContext,
                "PartiallyUpdateFridgeModelForFridge",
                values: new { fridgeId, id }),
                "partially_update_fridge_model",
                "PATCH")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForFridgeModels(HttpContext httpContext,
            LinkCollectionWrapper<Entity> fridgeModelsWrapper)
        {
            fridgeModelsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext,
                "GetFridgeModelsForFridge",
                values: new { }),
                "self",
                "GET"));

            return fridgeModelsWrapper;
        }
    }
}
