using Neo4j1.Configuration;
using Neo4j1.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j1.DataLayerService
{
    public class DataLayerService
    {

            private PictureRepository pictureRepository;
            private FoodRepository foodRepository;
            private UserRepository userRepository;
            private RestaurantRepository restaurantRepository;

            private IConfig config;

            public DataLayerService()
            {
                config = new LocalConfig();
                pictureRepository = new PictureRepository(config);
                foodRepository = new FoodRepository(config);
                userRepository = new UserRepository(config);
                restaurantRepository = new RestaurantRepository(config);
            }

            public PictureRepository PictureRepository
            {
                get
                {
                    return pictureRepository;
                }
            }

            public FoodRepository FoodRepository
            {
                get
                {
                    return foodRepository;
                }
            }

            public UserRepository UserRepository
            {
                get
                {
                    return userRepository;
                }
            }

            public RestaurantRepository RestaurantRepository
            {
                get
                {
                    return restaurantRepository;
                }
            }
        }
    
}
